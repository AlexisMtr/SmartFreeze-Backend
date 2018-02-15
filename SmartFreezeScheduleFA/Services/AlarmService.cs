using System;
using System.Collections.Generic;
using System.Linq;
using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using WeatherLibrary.Algorithmes.Freeze;
using static WeatherLibrary.Algorithmes.Freeze.FreezeForecast;

namespace SmartFreezeScheduleFA.Services
{
    public class AlarmService
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly IFreezeRepository freezeRepository;

        public AlarmService(IDeviceRepository deviceRepository, IFreezeRepository freezeRepository)
        {
            this.deviceRepository = deviceRepository;
            this.freezeRepository = freezeRepository;
        }

        public Alarm CreateCommunicationAlarm(string deviceId, string siteId, DateTime lastCommunication, Alarm.Gravity gravity)
        {
            return CreateAlarm(deviceId, siteId, Alarm.Type.CommuniationFailure, gravity,
                "Erreur de communication", $"Le capteur n'a pas communiqué depuis le {lastCommunication.ToString("dd/MM/yyyy HH:mm")}", null, null);
        }

        public Dictionary<DateTime, FreezeForecast.FreezingProbability> CalculAverageFreezePrediction12h(Dictionary<DateTime, FreezeForecast.FreezingProbability> predictions3h)
        {
            Dictionary<DateTime, FreezeForecast.FreezingProbability> averageFreezePrediction12h = new Dictionary<DateTime, FreezeForecast.FreezingProbability>();

            DateTime start = new DateTime();

            if (predictions3h.First().Key.Hour < 12)
            {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0); //AM
            }
            else
            {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0); //PM
            }
            DateTime end = start.AddHours(12);
            //compte le nombre de demie-journée
            double halfDayNumber = ((predictions3h.Last().Key) - (predictions3h.First().Key)).TotalDays;


            while (start <= predictions3h.Last().Key)
            {
                //prédictions pour la demie-journée en cours
                Dictionary<DateTime, int> predictionsForOneHalfDay = predictions3h
                    .Where(e => e.Key >= start && e.Key < end)
                    .ToDictionary(k => k.Key, v => (int)v.Value);

                //calcule la moyenne des prédictions sur cette demie-journée
                double avg = predictionsForOneHalfDay.Values.Average();
                double avgRounded = Math.Round(avg);

                averageFreezePrediction12h.Add(start, (FreezeForecast.FreezingProbability)(avgRounded));

                start = start.AddHours(12);
                end = end.AddHours(12);
            }
            return averageFreezePrediction12h;
        }

        public void CreateFreezeAlarm(string deviceId, string siteId, Dictionary<DateTime, FreezeForecast.FreezingProbability> dicoEntree)
        {
            Freeze lastFreeze = freezeRepository.GetLastFreezeByDevice(deviceId); //dernière mesure freeze + indiceDeConfiance enregistrée
            bool haveToCheckEndDate = false;
            bool haveToCheckEndDateForProlongation = false;
            bool checkForEndOFGel = false;
            bool lookingForEnd = false;
            DateTime startFreeze = DateTime.UtcNow;
            DateTime startFreezeV2 = DateTime.UtcNow;
            KeyValuePair<DateTime, FreezingProbability>? previousPrediction = null;
            IList<Alarm> crossAlarms;

            foreach (var prediction in dicoEntree)
            {
                if(!previousPrediction.HasValue)
                {
                    //première valeur
                    if (!haveToCheckEndDate && !haveToCheckEndDateForProlongation)
                    {
                        if(lastFreeze != null)
                        {
                            //si la prédiction est à 0 et que dernière freeze en base est de plus de 1 alors alarm degel
                            if ((int)prediction.Value == 0 && (lastFreeze.TrustIndication > 1))//OK
                            {//DEGEL
                                CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "degel prévu", "degel prévu le : " + prediction.Key,prediction.Key,null);
                            }
                            else if((int)prediction.Value > 1 && lastFreeze.TrustIndication == 0) 
                            {//GEL
                                startFreeze = prediction.Key;
                                haveToCheckEndDate = true; //puis boucler pour avoir la date de fin
                            }
                            else if ((int)prediction.Value > 1 && lastFreeze.TrustIndication > 1)//alors on parcours jusqu'à ce qu'on tombe sur 1 ou 0 et on update l'alarme
                            {//prolongation GEL de la dernière alarme
                                haveToCheckEndDateForProlongation = true; //puis boucler pour avoir la date de fin
                            }
                        }
                        else
                        {
                            if((int)prediction.Value > 1)
                            {
                                lookingForEnd = true;
                            }
                        }

                    }
                }
                else
                {
                    //à partir du deuxième élément
                    if (haveToCheckEndDate && (int)prediction.Value < 1)//OK
                    {   
                        //méthode pour vérifier qu'il n'existe pas une alarm qui croise celle là
                        crossAlarms = deviceRepository.GetCrossAlarmsByDevice(deviceId, startFreeze, prediction.Key);
                        //update la première alarm de la liste
                        deviceRepository.UpdateAlarm(deviceId, crossAlarms.First().Id, startFreeze, prediction.Key);
                        //supprime le reste des alarms en base
                        crossAlarms.RemoveAt(0);
                        foreach (var alarm in crossAlarms)
                        {
                            deviceRepository.deleteAlarmById(deviceId, alarm.Id);
                        }
                        //degel
                        CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "degel prévu", "degel prévu le " + prediction.Key, prediction.Key, null);
                        haveToCheckEndDate = false;
                    }else if (haveToCheckEndDate && prediction.Key == dicoEntree.Last().Key)//OK
                    {
                        IList<Alarm> alarms = deviceRepository.GetCrossAlarmsByDevice(deviceId, startFreezeV2, prediction.Key);
                        if (alarms != null && alarms.Any())
                        {
                            //méthode pour vérifier qu'il n'existe pas une alarm qui croise celle là
                            crossAlarms = deviceRepository.GetCrossAlarmsByDevice(deviceId, startFreeze, prediction.Key);
                            //update la première alarm de la liste
                            deviceRepository.UpdateAlarm(deviceId, crossAlarms.First().Id, startFreeze, prediction.Key);
                            //supprime le reste des alarms en base
                            crossAlarms.RemoveAt(0);
                            foreach (var alarm in crossAlarms)
                            {
                                deviceRepository.deleteAlarmById(deviceId, alarm.Id);
                            }
                        }else
                            {
                            CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "gel prévu", "gel prévu du " + startFreeze + " au " + previousPrediction.Value.Key, startFreezeV2, previousPrediction.Value.Key);
                        }
                        haveToCheckEndDate = false;
                    }
                    else if (haveToCheckEndDateForProlongation && ((int)prediction.Value < 1))
                    {//vérifie si il y a déjà eu un dégel si oui update sinon créer TODO
                        IList<Alarm> alarms = deviceRepository.GetCrossAlarmsByDevice(deviceId, startFreezeV2, prediction.Key);
                        if (alarms != null && alarms.Any())
                        {
                            //méthode pour vérifier qu'il n'existe pas une alarm qui croise celle là
                            crossAlarms = deviceRepository.GetCrossAlarmsByDevice(deviceId, prediction.Key, prediction.Key);
                            //update la première alarm de la liste
                            deviceRepository.UpdateAlarm(deviceId, crossAlarms.First().Id, startFreeze, prediction.Key);
                            //supprime le reste des alarms en base
                            crossAlarms.RemoveAt(0);
                            foreach (var alarm in crossAlarms)
                            {
                                deviceRepository.deleteAlarmById(deviceId, alarm.Id);
                            }
                        }
                        else
                        {
                            CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "degel prévu", "degel prévu le " + prediction.Key, prediction.Key, null);
                        }
                        haveToCheckEndDateForProlongation = false;
                    }
                    else if ((int)prediction.Value > 1 && !checkForEndOFGel && !lookingForEnd && !haveToCheckEndDate && !haveToCheckEndDateForProlongation)//OK
                    {   //créer alarm de GEL puis boucler pour avoir la fin et créer alarme de dégel
                        startFreezeV2 = prediction.Key;
                        checkForEndOFGel = true;

                    }
                    else if ((checkForEndOFGel && prediction.Value == FreezingProbability.ZERO))
                    {   //créer alarm de gel
                        CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "gel prévu", "gel prévu du " + startFreezeV2 + " au " + previousPrediction.Value.Key, startFreezeV2, previousPrediction.Value.Key);
                        // créer alam de dégel
                        CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "degel prévu", "degel prévu le " + prediction.Key, prediction.Key, null);
                        checkForEndOFGel = false;
                    }
                    else if ((checkForEndOFGel && prediction.Key == dicoEntree.Last().Key))//OK
                    {
                        IList<Alarm> alarms = deviceRepository.GetCrossAlarmsByDevice(deviceId, startFreezeV2, prediction.Key);
                        // TODO empêcher l'update si alarm est identique : if (alarms.First().Start == startFreezeV2 && alarms.First().End == prediction.Key)
                        if (alarms != null && alarms.Any())
                        {
                            //update la première alarm de la liste
                            deviceRepository.UpdateAlarm(deviceId, alarms.First().Id, startFreezeV2, prediction.Key);
                            //supprime le reste des alarms en base
                            alarms.RemoveAt(0);
                            foreach (var alarm in alarms)
                            {
                                deviceRepository.deleteAlarmById(deviceId, alarm.Id);
                            }

                        }
                        else
                        {
                            //créer alarm de gel
                            CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "gel prévu", "gel prévu du " + startFreezeV2 + " au " + prediction.Key, startFreezeV2, prediction.Key);
                        }
                        checkForEndOFGel = false;
                    }
                    else if (lookingForEnd && prediction.Value == FreezingProbability.ZERO)
                    {
                        //créer alarm de gel
                        CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "gel prévu", "gel prévu du " + dicoEntree.First().Key + " au " + previousPrediction.Value.Key, dicoEntree.First().Key, previousPrediction.Value.Key);
                        // créer alam de dégel
                        CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "degel prévu", "degel prévu le " + prediction.Key, prediction.Key, null);
                        lookingForEnd = false;
                    }
                }
                previousPrediction = prediction;
            }
        }

        public Alarm CreateAlarm(string deviceId, string siteId, Alarm.Type alarmType, Alarm.Gravity alarmGravity, string shortDescription, string description, DateTime? start, DateTime? end)
        {
            Alarm alarm = new Alarm()
            {
                Id = $"{deviceId}-alarm{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}",
                DeviceId = deviceId,
                SiteId = siteId,
                IsActive = true,
                AlarmType = alarmType,
                AlarmGravity = alarmGravity,
                OccuredAt = DateTime.UtcNow,
                ShortDescription = shortDescription,
                Description = description,
                Start = start,
                End = end

            };
            deviceRepository.AddAlarm(deviceId, alarm);

            return alarm;
        }

        public void UpdateAlarm(String deviceId, Alarm alarm)
        {
            deviceRepository.UpdateStatusAlarm(deviceId, alarm);
        }

    }
}
