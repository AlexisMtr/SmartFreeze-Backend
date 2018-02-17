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

                averageFreezePrediction12h.Add(start, (FreezingProbability)(avgRounded));

                start = start.AddHours(12);
                end = end.AddHours(12);
            }
            return averageFreezePrediction12h;
        }

        public void CreateFreezeAlarm(string deviceId, string siteId, Dictionary<DateTime, FreezingProbability> dicoEntree)
        {
            Freeze lastFreeze = freezeRepository.GetLastFreezeByDevice(deviceId); //dernière mesure freeze + indiceDeConfiance enregistrée
            bool haveToCheckEndDate = false;
            bool haveToCheckEndDateForProlongation = false;
            bool checkForEndOFGel = false;
            bool lookingForEnd = false;

            DateTime startFreezeDate = DateTime.UtcNow;
            KeyValuePair<DateTime, FreezingProbability>? previousPrediction = null;

            foreach (var prediction in dicoEntree)
            {
                if (!previousPrediction.HasValue)
                {
                    ProcessFirstPrediction(prediction, lastFreeze, siteId, out haveToCheckEndDate, out haveToCheckEndDateForProlongation, out lookingForEnd);
                }
                else
                {
                    if (haveToCheckEndDate && (int)prediction.Value < 1)
                    {
                        ManageAlarm(deviceId, siteId, dicoEntree.First().Key, prediction.Key,
                            $"Le capteur {deviceId} detecte un gel du {dicoEntree.First().Key.ToString("dd/MM/yyyy")} au {previousPrediction.Value.Key.ToString("dd/MM/yyyy")}",
                            $"Gel prévu du {dicoEntree.First().Key.ToString("dd/MM/yyyy")} au {previousPrediction.Value.Key.ToString("dd/MM/yyyy")}");
                        haveToCheckEndDate = false;
                        // TODO : Alarm de degel
                    }
                    else if (haveToCheckEndDate && prediction.Key == dicoEntree.Last().Key)
                    {
                        ManageAlarm(deviceId, siteId, dicoEntree.First().Key, prediction.Key,
                            $"Le capteur {deviceId} detecte du gel du {dicoEntree.First().Key.ToString("dd/MM/yyyy")} au {previousPrediction.Value.Key.ToString("dd/MM/yyyy")}",
                            $"Gel prévu du {dicoEntree.First().Key.ToString("dd/MM/yyyy")} au {prediction.Key.ToString("dd/MM/yyyy")}");
                        haveToCheckEndDate = false;
                    }
                    else if (haveToCheckEndDateForProlongation && ((int)prediction.Value < 1))
                    {
                        ManageAlarm(deviceId, siteId, prediction.Key, prediction.Key,
                            $"Le capteur {deviceId} detecte un dégel à partir du {prediction.Key.ToString("dd/MM/yyyy")}",
                            $"Dégel prévu le {prediction.Key.ToString("dd/MM/yyyy")}");
                        haveToCheckEndDateForProlongation = false;
                    }
                    else if (haveToCheckEndDateForProlongation && prediction.Key == dicoEntree.Last().Key)
                    {
                        ManageAlarm(deviceId, siteId, dicoEntree.First().Key, prediction.Key,
                            $"Le capteur {deviceId} detecte du gel du {dicoEntree.First().Key.ToString("dd/MM/yyyy")} au {prediction.Key.ToString("dd/MM/yyyy")}",
                            $"Dégel prévu le {dicoEntree.First().Key.ToString("dd/MM/yyyy")}");
                        haveToCheckEndDateForProlongation = false;
                    }
                    else if ((int)prediction.Value > 1 && !checkForEndOFGel && !lookingForEnd && !haveToCheckEndDate && !haveToCheckEndDateForProlongation)
                    {   
                        startFreezeDate = prediction.Key;
                        checkForEndOFGel = true;
                    }
                    else if ((checkForEndOFGel && prediction.Value == FreezingProbability.ZERO))
                    {
                        ManageAlarm(deviceId, siteId, startFreezeDate, previousPrediction.Value.Key,
                            $"Le capteur {deviceId} detecte du gel du {startFreezeDate.ToString("dd/MM/yyyy")} au {previousPrediction.Value.Key.ToString("dd/MM/yyyy")}",
                            $"Gel prévu du {startFreezeDate.ToString("dd/MM/yyyy")} au {previousPrediction.Value.Key.ToString("dd/MM/yyyy")}");
                        
                        CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical,
                            $"Dégel prévu le {prediction.Key.ToString("dd/MM/yyyy")}",
                            $"Le capteur {deviceId} detecte un dégel à partir du {prediction.Key.ToString("dd/MM/yyyy")}",
                            prediction.Key, null);

                        checkForEndOFGel = false;
                    }
                    else if ((checkForEndOFGel && prediction.Key == dicoEntree.Last().Key))
                    {
                        ManageAlarm(deviceId, siteId, startFreezeDate, prediction.Key,
                            $"Le capteur {deviceId} détecte du gel du {startFreezeDate.ToString("dd/MM/yyyy")} au {prediction.Key.ToString("dd/MM/yyyy")}",
                            $"Gel prévu du {startFreezeDate.ToString("dd/MM/yyyy")} au {prediction.Key.ToString("dd/MM/yyyy")}");
                        checkForEndOFGel = false;
                    }
                    else if (lookingForEnd && prediction.Value == FreezingProbability.ZERO)
                    {
                        CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical,
                            $"Gel prévu du {dicoEntree.First().Key.ToString("dd/MM/yyyy")} au {dicoEntree.First().Key.ToString("dd/MM/yyyy")}",
                            $"Le capteur {deviceId} détecte du gel du {dicoEntree.First().Key.ToString("dd/MM/yyyy")} au {previousPrediction.Value.Key.ToString("dd/MM/yyyy")}",
                            dicoEntree.First().Key,
                            previousPrediction.Value.Key);
                        
                        CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical,
                            $"Dégel prévu le {prediction.Key.ToString("dd/MM/yyyy")}",
                            $"Le capteur {deviceId} detecte un dégel à partir du {prediction.Key.ToString("dd/MM/yyyy")}",
                            prediction.Key, null);
                        lookingForEnd = false;
                    }
                    else if (lookingForEnd && prediction.Key == dicoEntree.Last().Key)
                    {
                        ManageAlarm(deviceId, siteId, dicoEntree.First().Key, prediction.Key,
                            $"Le capteur {deviceId} détecte du gel du {dicoEntree.First().Key.ToString("dd/MM/yyyy")} au {prediction.Key.ToString("dd/MM/yyyy")}",
                            $"Gel prévu du {dicoEntree.First().Key.ToString("dd/MM/yyyy")} au {prediction.Key.ToString("dd/MM/yyyy")}");
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


        private void ManageAlarm(string deviceId, string siteId, DateTime start, DateTime end, string description, string shortDescription)
        {
            IList<Alarm> alarms = deviceRepository.GetCrossAlarmsByDevice(deviceId, start, end);
            if (alarms != null && alarms.Any())
            {
                //update la première alarm de la liste
                deviceRepository.UpdateAlarm(deviceId, alarms.First().Id, start, end);
                //supprime le reste des alarms en base
                alarms.RemoveAt(0);
                foreach (var alarm in alarms)
                {
                    deviceRepository.DeleteAlarmById(deviceId, alarm.Id);
                }
            }
            else
            {
                CreateAlarm(deviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, shortDescription, description, start, end);
            }
        }


        private void ProcessFirstPrediction(KeyValuePair<DateTime, FreezingProbability> firstPrediction, Freeze previousFreeze, string siteId,
            out bool haveToCheckEndDate, out bool haveToCheckEndDateForProlongation, out bool lookingForEnd)
        {
            haveToCheckEndDate = false;
            haveToCheckEndDateForProlongation = false;
            lookingForEnd = false;

            if (previousFreeze != null)
            {
                if (firstPrediction.Value == FreezingProbability.ZERO && (previousFreeze.TrustIndication > 1))
                {
                    CreateAlarm(previousFreeze.DeviceId, siteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical,
                        $"Dégel prévu le {firstPrediction.Key.ToString("dd/MM/yyyy")}",
                        $"Le capteur {previousFreeze.DeviceId} detecte un dégel à partir du {firstPrediction.Key.ToString("dd/MM/yyyy")}", firstPrediction.Key, null);
                }
                else if ((int)firstPrediction.Value > 1 && previousFreeze.TrustIndication == 0)
                {
                    haveToCheckEndDate = true;
                }
                else if ((int)firstPrediction.Value > 1 && previousFreeze.TrustIndication > 1)
                {
                    haveToCheckEndDateForProlongation = true;
                }
            }
            else
            {
                if ((int)firstPrediction.Value > 1)
                {
                    lookingForEnd = true;
                }
            }
        }

    }
}
