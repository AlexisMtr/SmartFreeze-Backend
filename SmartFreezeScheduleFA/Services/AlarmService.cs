using System;
using System.Collections.Generic;
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

        internal Dictionary<DateTime, FreezingProbability> CalculAverageFreezePrediction12h(Dictionary<DateTime, FreezingProbability> freezingProbabilityList)
        {
            throw new NotImplementedException();
        }
    }
}
