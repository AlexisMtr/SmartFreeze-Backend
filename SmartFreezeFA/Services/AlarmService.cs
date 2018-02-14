
using SmartFreezeFA.Models;
using SmartFreezeFA.Repositories;
using System;

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

            if (humidity > 80)
            {
                description = "L'humidité intérieur est anormalement élevée";
                shortDescription = "humidité > 80";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }

            if (humidity < 20)
            {
                description = "L'humidité intérieur est anormalement basse";
                shortDescription = "humidité < 20";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Critical, shortDescription, description);
            }



            else if ((humidity >= 20) && (humidity < 30))
            {
                description = "L'humidité intérieur est critique";
                shortDescription = "humidité entre 20 et 30";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Serious, shortDescription, description);
            }

            else if ((humidity > 70) && (humidity < 80))
            {
                description = "L'humidité intérieur est critique";
                shortDescription = "humidité entre 70 et 80";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Serious, shortDescription, description);
            }

            else if ((humidity >= 30) && (humidity < 39)) 
            {
                description = "L'humidité intérieur est anormale";
                shortDescription = "humidité entre 30 et 39";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Information, shortDescription, description);
            }


            else if ((humidity > 60) && (humidity <= 70))
            {
                description = "L'humidité intérieur est anormale";
                shortDescription = "humidité entre 60 et 70";
                CreateAlarm(deviceId, null, Alarm.Type.DeviceFailure, Alarm.Gravity.Information, shortDescription, description);

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
            string ShortDescription, Description = null;
            if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.15))
            {
                //créer alarme level 3 15%
                ShortDescription = "batterie < 15%";
                Description = "Batterie très faible pour le capteur (moins de 15%)";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.BatteryWarning, Alarm.Gravity.Critical, ShortDescription, Description);
            }
            else if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.3))
            {
                //créer alarme level 2 30%
                ShortDescription = "batterie < 30%";
                Description = "Batterie faible pour le capteur (moins de 30%)";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.BatteryWarning, Alarm.Gravity.Critical, ShortDescription, Description);

            }
            else if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.5))
            {
                //créer alarme level 1 50%
                ShortDescription = "batterie < 50%";
                Description = "Batterie à 50% pour le capteur";
                CreateAlarm(telemetry.DeviceId, null, Alarm.Type.BatteryWarning, Alarm.Gravity.Critical, ShortDescription, Description);

            }
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
