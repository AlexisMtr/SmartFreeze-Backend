
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

        const double maxVoltageValue = 3.6;

        public AlarmService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public void CreateHumidityAlarm(Telemetry telemetry)
        {

            double humidity = telemetry.Humidity;
            string deviceId = telemetry.DeviceId;

       
            string shortDescription, description = null;

            if (humidity > 100)
            {
                description = "Donnée d'humidité anormale";
                shortDescription = $"L'humidité du capteur {telemetry.DeviceId} est supérieure à 100% ({telemetry.Temperature})";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }

            else if (humidity <=0)
            {
                description = "Donnée d'humidité anormale";
                shortDescription = $"L'humidité du capteur {telemetry.DeviceId} est inférieure à 0% ({telemetry.Temperature})";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }

        }

        public void CreateTemperatureAlarm(Telemetry telemetry)
        {

            double temperature = telemetry.Temperature;
            string deviceId = telemetry.DeviceId;
            string shortDescription = string.Empty;
            string description = string.Empty;

            if (temperature > 100)
            {
                description = "Donnée de température anormale";
                shortDescription = $"La température du capteur {telemetry.DeviceId} est supérieure à 100°C ({telemetry.Temperature})";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }

            if (temperature < -300)
            {
                description = "Donnée de température anormale";
                shortDescription = $"La température du capteur {telemetry.DeviceId} est inférieure à -300°C ({telemetry.Temperature})";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }

            else if ((temperature >= -300) && (temperature < -100))
            {
                description = "Donnée de température anormale";
                shortDescription = $"La température du capteur {telemetry.DeviceId} est inférieure à -100°C ({telemetry.Temperature})";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Serious, shortDescription, description);
            }

            else if  ((temperature > 80) && (temperature < 100))
            {
                description = "Donnée de température anormale";
                shortDescription = $"La température du capteur {telemetry.DeviceId} est supérieure à 80°C ({telemetry.Temperature})";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Serious, shortDescription, description);
            }

            else if ((temperature > -99) && (temperature < -50))
            {
                description = "Donnée de température anormale";
                shortDescription = $"La température du capteur {telemetry.DeviceId} est inférieure à -50°C ({telemetry.Temperature})";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Information, shortDescription, description);
             }

            else if  ((temperature > 50) && (temperature < 79))
            {
                description = "Donnée de température anormale";
                shortDescription = $"La température du capteur {telemetry.DeviceId} est supérieure à 50°C ({telemetry.Temperature})";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Information, shortDescription, description);
            }

        }

        public void CreatePressureAlarm(Telemetry telemetry)
        {
            // TODO : Add check on Pressure value
        }

        public void CreateBatteryAlarm(Telemetry telemetry)
        {
            string shortDescription = string.Empty;
            string description = string.Empty;

            if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.15))
            {
                //créer alarme level 3 15%
                shortDescription = "Niveau de batterie critique";
                description = $"La batterie du capteur {telemetry.DeviceId} est inférieur à 15% ({telemetry.BatteryVoltage}V)";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }
            else if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.3))
            {
                //créer alarme level 2 30%
                shortDescription = "Niveau de batterie faible";
                description = $"La batterie du capteur {telemetry.DeviceId} est inférieur à 30% ({telemetry.BatteryVoltage}V)";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Serious, shortDescription, description);

            }
            else if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.5))
            {
                //créer alarme level 1 50%
                shortDescription = "Niveau de batterie faible";
                description = $"La batterie du capteur {telemetry.DeviceId} est inférieur à 50% ({telemetry.BatteryVoltage}V)";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Information, shortDescription, description);

            }
        }

        public void CreateFreezingAlarm(Telemetry telemetry, DateTime? start, DateTime? end)
        {
            string shortDescription = string.Empty;
            string description = string.Empty;

            if (start.HasValue)
            {
                shortDescription = "Gel détecté";
                description = $"Le capteur {telemetry.DeviceId} detecte du gel";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, shortDescription, description);
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


        private void CreateAlarm(string DeviceId, string SiteId, Alarm.Type AlarmType, Alarm.Gravity AlarmGravity, string shortDescription, string description)
        {
            Alarm alarm = new Alarm()
            {
                Id = DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                DeviceId = DeviceId,
                SiteId = SiteId,
                IsActive = true,
                AlarmType = AlarmType,
                AlarmGravity = AlarmGravity,
                OccuredAt = DateTime.UtcNow,
                ShortDescription = shortDescription,
                Description = description
            };

            deviceRepository.AddAlarm(DeviceId, alarm);
        }

    }
}
