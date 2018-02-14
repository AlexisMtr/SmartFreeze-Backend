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

        public AlarmService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
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

        public void createFreezeAlarm(string deviceId, string siteId, Dictionary<DateTime, FreezeForecast.FreezingProbability> dicoPredictionBy12h)
        {//créé des alarmes de freeze (date début et date fin) et thaw (date début)
            Freeze lastFreeze = freezeRepository.getLastFreezeByDevice(deviceId);
            bool haveToCheckEndDate = false;
            DateTime startFreeze = DateTime.UtcNow;
            KeyValuePair<DateTime, FreezingProbability>? previousPrediction = null;
            IList<Alarm> crossAlarms;

            foreach (var prediction in dicoPredictionBy12h)
            {
                if(!previousPrediction.HasValue)
                {//à partir de la première valeur
                    if (!haveToCheckEndDate)
                    {
                        //si la prédiction est à 0 et que dernière freeze en base est de plus de 1 alors alarm degel
                        if ((int)prediction.Value == 0 && (lastFreeze.TrustIndication > 1))
                        {//DEGEL
                            CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "degel prévu", "degel prévu le : " + prediction.Key,prediction.Key,null);
                        }else if((int)prediction.Value > 1 && lastFreeze.TrustIndication == 0) //&& 0 avant alors debut de gel 
                        {//GEL
                            startFreeze = prediction.Key;
                            haveToCheckEndDate = true; //puis boucler pour avoir la date de fin
                        }
                        else if (true)//&& >1 avant alors on parcours jusqu'à ce qu'on tombe sur 1 ou 0 et on update l'alarme
                        {

                        }else 
                        {

                        }

                    }
                    else
                    {
                        if(haveToCheckEndDate && (int)prediction.Value < 1)
                        {
                            //méthode pour vérifier qu'il n'existe pas une alarm qui croise celle là
                            crossAlarms = deviceRepository.GetCrossAlarmsByDevice(deviceId, startFreeze, prediction.Key);
                            //update crossAlarms.First()
                            //deviceRepository.
                            //crossAlarms.RemoveAt(0);
                            //boucler sur la liste et update la première et delete les autres
                            //CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, "gel prévu", "gel prévu du " + startFreeze + " au " + prediction.Key, startFreeze, prediction.Key);
                        }
                    }
                }
                else//à partir du deuxième élément
                {
                    if ((int)prediction.Value == 0 && ((int)previousPrediction.Value.Value < 2))
                    {
                        // rien
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

    }
}
