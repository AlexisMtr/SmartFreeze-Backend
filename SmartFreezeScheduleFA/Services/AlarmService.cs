using System;
using System.Collections.Generic;
using System.Linq;
using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using static WeatherLibrary.Algorithmes.Freeze.FreezeForecast;

namespace SmartFreezeScheduleFA.Services
{
    public class AlarmService
    {
        private readonly IDeviceRepository deviceRepository;

        public AlarmService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public Alarm CreateCommunicationAlarm(string deviceId, string siteId, DateTime lastCommunication, Alarm.Gravity gravity)
        {
            return CreateAlarm(deviceId, siteId, Alarm.Type.CommuniationFailure, gravity,
                "Erreur de communication", $"Le capteur n'a pas communiqué depuis le {lastCommunication.ToString("dd/MM/yyyy HH:mm")}");
        }

        public Alarm CreateAlarm(string deviceId, string siteId, Alarm.Type alarmType, Alarm.Gravity alarmGravity, string shortDescription, string description)
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
                Description = description
            };
            deviceRepository.AddAlarm(deviceId, alarm);

            return alarm;
        }

        public Dictionary<DateTime, FreezingProbability> CalculAverageFreezePrediction12h(Dictionary<DateTime, FreezingProbability> freezingProbabilityList)
        {
            Dictionary<DateTime, FreezingProbability> averageFreezePrediction12h = new Dictionary<DateTime, FreezingProbability>();

            DateTime start = new DateTime();

            if (freezingProbabilityList.First().Key.Hour < 12)
            {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0); //AM
            }
            else
            {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0); //PM
            }
            DateTime end = start.AddHours(12);

            while (start <= freezingProbabilityList.Last().Key)
            {
                //prédictions pour la demie-journée en cours
                Dictionary<DateTime, int> predictionsForOneHalfDay = freezingProbabilityList
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
    }
}
