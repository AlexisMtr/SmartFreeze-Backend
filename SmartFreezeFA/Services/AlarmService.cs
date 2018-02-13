
using SmartFreezeFA.Models;
using SmartFreezeFA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

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
                description = "L'humidité est anormalement élevée";
                shortDescription = "humidité > 100";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }

            else if (humidity <=0)
            {
                description = "L'humidité est anormalement basse";
                shortDescription = "humidité <=0";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }

        }

        public void CreateTemperatureAlarm(Telemetry telemetry)
        {

            double temperature = telemetry.Temperature;
            string deviceId = telemetry.DeviceId;
            string shortDescription, description = null;

                if (temperature > 100)
                {
                    description = "La température  est anormalement hausse";
                    shortDescription = "température > 100";
                    CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
                }

            if (temperature < -300)
            {
                    description = "La température  est anormalement basse";
                    shortDescription = "température <-300";
                    CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }

            else if ((temperature >= -300) && (temperature < -100))
            {
                    description = "La température  est critique";
                    shortDescription = "température entre -300 et -100";
                    CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Serious, shortDescription, description);
            }

            else if  ((temperature > 80) && (temperature < 100))
            {
                description = "La température  est critique";
                shortDescription = "température entre 80 et 100";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Serious, shortDescription, description);
            }

            else if ((temperature > -99) && (temperature < -50)) 
            {
                    description = "La température  est anormale";
                    shortDescription = "température entre -99 et -50";
                    CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Information, shortDescription, description);
             }

            else if  ((temperature > 50) && (temperature < 79))
            {
                description = "La température  est anormale";
                shortDescription = "température entre 50 et 79";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Information, shortDescription, description);
            }

        }

        public void CreatePressureAlarm(Telemetry telemetry)
        {
            // TODO : Add check on Pressure value
        }

        public void CreateBatteryAlarm(Telemetry telemetry)
        {
            string shortDescription, description = null;
            if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.15))
            {
                //créer alarme level 3 15%
                shortDescription = "batterie < 15%";
                description = "Batterie très faible pour le capteur (moins de 15%)";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }
            else if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.3))
            {
                //créer alarme level 2 30%
                shortDescription = "batterie < 30%";
                description = "Batterie faible pour le capteur (moins de 30%)";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Serious, shortDescription, description);

            }
            else if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.5))
            {
                //créer alarme level 1 50%
                shortDescription = "batterie < 50%";
                description = "Batterie à 50% pour le capteur";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Information, shortDescription, description);

            }
        }

        public void CreateFreezingAlarm(Telemetry telemetry, DateTime? start, DateTime? end)
        {
            String shortDescription = "", description = "";

            if (start.HasValue)
            {
                shortDescription = "Gel!";
                description = "Le capteur detecte du gel";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, shortDescription, description);
            }

        }

        public Dictionary<DateTime, object> CalculAverageFreezePrediction12h(Dictionary<DateTime, object> predictions3h)
        {
            Dictionary<DateTime, object> averageFreezePrediction12h = new Dictionary<DateTime, object>();
            //List<int> averagePredictions = new List<int>();

            DateTime start = new DateTime();
            
            if(predictions3h.First().Key.Hour < 12)
            {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0); //matin
            }
            else
            {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0); //aprèm
            }
            DateTime end = start.AddHours(12);

            foreach (var prediction3h in predictions3h)
            {
                Dictionary<DateTime, object> predictionsForOneHalfDay = predictions3h
                    .Where(e => e.Key > start && e.Key < end)
                    .ToDictionary(k => k.Key, v => v.Value);
                //calculer la moyenne des prédictions sur cette demie-journée

                start.AddHours(12);
                end.AddHours(12);

                // TODO créer entrées en base (prédictions) + créer tableau résultant

            }
            return averageFreezePrediction12h;
        }


        private void CreateAlarm(string DeviceId, string SiteId, Alarm.Type AlarmType, Alarm.Gravity AlarmGravity, string shortDescription, string description)
        {
            var list = new List<int>();
            list.Average();
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
