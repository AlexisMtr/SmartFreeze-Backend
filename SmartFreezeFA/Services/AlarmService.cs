
using SmartFreezeFA.Models;
using SmartFreezeFA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherLibrary.Algorithmes.Freeze;

namespace SmartFreezeFA.Services
{
    public class AlarmService
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly IAlarmRepository alarmRepository;

        const double maxVoltageValue = 3.6;

        public AlarmService(IDeviceRepository deviceRepository, IAlarmRepository alarmRepository)
        {
            this.deviceRepository = deviceRepository;
            this.alarmRepository = alarmRepository;
        }

        public void CreateHumidityAlarm(Telemetry telemetry, string siteId)
        {
            alarmRepository.SetAsInactive(telemetry.DeviceId, Alarm.AlarmSubtype.Humidity);
            
            if (telemetry.Humidity > 100)
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Humidity, Alarm.Gravity.Critical, "Donnée d'humidité anormale",
                    $"L'humidité du capteur {telemetry.DeviceId} est supérieure à 100% ({telemetry.Temperature})");
            }

            else if (telemetry.Humidity <= 0)
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Humidity, Alarm.Gravity.Critical, "Donnée d'humidité anormale",
                    $"L'humidité du capteur {telemetry.DeviceId} est inférieure à 0% ({telemetry.Temperature})");
            }

        }

        public void CreateTemperatureAlarm(Telemetry telemetry, string siteId)
        {
            alarmRepository.SetAsInactive(telemetry.DeviceId, Alarm.AlarmSubtype.Temperature);
            
            if (telemetry.Temperature > 100)
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Temperature, Alarm.Gravity.Critical, "Donnée de température anormale",
                    $"La température du capteur {telemetry.DeviceId} est supérieure à 100°C ({telemetry.Temperature})");
            }

            if (telemetry.Temperature < -300)
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Temperature, Alarm.Gravity.Critical, "Donnée de température anormale",
                    $"La température du capteur {telemetry.DeviceId} est inférieure à -300°C ({telemetry.Temperature})");
            }

            else if ((telemetry.Temperature >= -300) && (telemetry.Temperature < -100))
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Temperature, Alarm.Gravity.Serious, "Donnée de température anormale",
                    $"La température du capteur {telemetry.DeviceId} est inférieure à -100°C ({telemetry.Temperature})");
            }

            else if ((telemetry.Temperature > 80) && (telemetry.Temperature < 100))
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Temperature, Alarm.Gravity.Serious, "Donnée de température anormale",
                    $"La température du capteur {telemetry.DeviceId} est supérieure à 80°C ({telemetry.Temperature})");
            }

            else if ((telemetry.Temperature > -99) && (telemetry.Temperature < -50))
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Temperature, Alarm.Gravity.Information, "Donnée de température anormale",
                    $"La température du capteur {telemetry.DeviceId} est inférieure à -50°C ({telemetry.Temperature})");
            }

            else if ((telemetry.Temperature > 50) && (telemetry.Temperature < 79))
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Temperature, Alarm.Gravity.Information, "Donnée de température anormale",
                    $"La température du capteur {telemetry.DeviceId} est supérieure à 50°C ({telemetry.Temperature})");
            }

        }

        public void CreateBatteryAlarm(Telemetry telemetry, string siteId)
        {
            alarmRepository.SetAsInactive(telemetry.DeviceId, Alarm.AlarmSubtype.Battery);
            
            if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.15))
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Battery, Alarm.Gravity.Critical, "Niveau de batterie critique",
                    $"La batterie du capteur {telemetry.DeviceId} est inférieur à 15% ({telemetry.BatteryVoltage}V)");
            }
            else if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.3))
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Battery, Alarm.Gravity.Serious, "Niveau de batterie faible",
                    $"La batterie du capteur {telemetry.DeviceId} est inférieur à 30% ({telemetry.BatteryVoltage}V)");

            }
            else if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.5))
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.DeviceFailure, Alarm.AlarmSubtype.Battery, Alarm.Gravity.Information, "Niveau de batterie faible",
                    $"La batterie du capteur {telemetry.DeviceId} est inférieur à 50% ({telemetry.BatteryVoltage}V)");

            }
        }

        // TODO : Check for previous freeze alarm
        public void CreateFreezingAlarm(Telemetry telemetry, string siteId, DateTime? start, DateTime? end)
        {
            Alarm latestFreezeAlarm = alarmRepository.GetLatestAlarmByType(telemetry.DeviceId, Alarm.Type.FreezeWarning, null);

            if (start.HasValue && latestFreezeAlarm == null)
            {
                CreateAlarm(telemetry.DeviceId, siteId, Alarm.Type.FreezeWarning, Alarm.AlarmSubtype.Freeze, Alarm.Gravity.Critical,
                    "Gel détecté", $"Le capteur {telemetry.DeviceId} detecte du gel");
            }

        }

        public void SetFreezeAlarmAsInactive(string deviceId)
        {
            alarmRepository.SetAsInactive(deviceId, Alarm.AlarmSubtype.Freeze);
        }

        public void CheckForActiveCommunicationFailureAlarms(string deviceId)
        {
            Alarm alarm = alarmRepository.GetLatestAlarmByType(deviceId, Alarm.Type.CommunicationError, null);
            if(alarm != null)
            {
                alarmRepository.SetAsInactive(alarm.Id);
            }
        }

        public Dictionary<DateTime, FreezeForecast.FreezingProbability> CalculAverageFreezePrediction12h(Dictionary<DateTime, FreezeForecast.FreezingProbability> predictions3h)
        {
            Dictionary<DateTime, FreezeForecast.FreezingProbability> averageFreezePrediction12h = new Dictionary<DateTime, FreezeForecast.FreezingProbability>();

            DateTime start = new DateTime();
            
            if(predictions3h.First().Key.Hour < 12)
            {
                start = new DateTime(predictions3h.First().Key.Year, predictions3h.First().Key.Month, predictions3h.First().Key.Day, 0, 0, 0); //AM
            }
            else
            {
                start = new DateTime(predictions3h.First().Key.Year, predictions3h.First().Key.Month, predictions3h.First().Key.Day, 12, 0, 0); //PM
            }
            DateTime end = start.AddHours(12);
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


        private void CreateAlarm(string deviceId, string siteId, Alarm.Type alarmType, Alarm.AlarmSubtype subtype, Alarm.Gravity alarmGravity, string shortDescription, string description)
        {
            Alarm alarm = new Alarm()
            {
                Id = $"{deviceId}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}",
                DeviceId = deviceId,
                SiteId = siteId,
                IsActive = true,
                AlarmType = alarmType,
                AlarmGravity = alarmGravity,
                OccuredAt = DateTime.UtcNow,
                ShortDescription = shortDescription,
                Description = description,
                Subtype = subtype
            };

            deviceRepository.AddAlarm(deviceId, alarm);
        }

    }
}
