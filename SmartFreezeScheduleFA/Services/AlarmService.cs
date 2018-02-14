using System;
using System.Collections.Generic;
using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using WeatherLibrary.Algorithmes.Freeze;

namespace SmartFreezeScheduleFA.Services
{
    public class AlarmService
    {
        private readonly IDeviceRepository deviceRepository;

        public AlarmService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public Alarm CreateCommunicationAlarm(string deviceId, Alarm.Gravity gravity)
        {
            string desc = string.Empty;
            string shortDesc = "Erreur de communication";

            switch (gravity)
            {
                case Alarm.Gravity.Information:
                    desc += "Le capteur n'a pas communiqué depuis plus d'une heure";
                    break;
                case Alarm.Gravity.Serious:
                    desc = "Le capteur n'a pas communiqué depuis plus de 4 heures";
                    break;
                case Alarm.Gravity.Critical:
                    desc = "Le capteur n'a pas communiqué depuis plus de 7 heures";
                    break;
                default:
                    break;
            }

            return CreateAlarm(deviceId, null, Alarm.Type.CommuniationFailure, gravity, shortDesc, desc);
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


        public bool UpdateAlarm(string deviceId, Alarm alarm)
        {
            return deviceRepository.UpdateAlarm(deviceId, alarm);
        }

        internal Dictionary<DateTime, FreezeForecast.FreezingProbability> CalculAverageFreezePrediction12h(Dictionary<DateTime, FreezeForecast.FreezingProbability> freezingProbabilityList)
        {
            throw new NotImplementedException();
        }
    }
}
