
using SmartFreezeFA.Models;
using SmartFreezeFA.Repositories;
using System;

namespace SmartFreezeFA.Services
{
    public class AlarmService
    {
        private readonly ITelemetryRepository telemetryRepository;
        private readonly IAlarmRepository alarmRepository;
        private readonly IDeviceRepository deviceRepository;

        const double maxVoltageValue = 3.6;

        public AlarmService(ITelemetryRepository telemetryRepository, IAlarmRepository alarmRepository, IDeviceRepository deviceRepository)
        {
            this.telemetryRepository = telemetryRepository;
            this.alarmRepository = alarmRepository;
            this.deviceRepository = deviceRepository;
        }

        public void CreateHumidityAlarm(Telemetry telemetry)
        {
            // TODO : Add check on Humidity value
        }

        public void CreateTemperatureAlarm(Telemetry telemetry)
        {
            // TODO : Add check on Temperature value
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

        private void CreateAlarm(string DeviceId, string SiteId, Alarm.Type AlarmType, Alarm.Gravity AlarmGravity, string ShortDescription, string Description)
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
                ShortDescription = ShortDescription,
                Description = Description
            };

            deviceRepository.AddAlarm(DeviceId, alarm);
        }

    }
}
