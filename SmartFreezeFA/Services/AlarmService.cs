
using SmartFreezeFA.Models;
using SmartFreezeFA.Repositories;

namespace SmartFreezeFA.Services
{
    public class AlarmService
    {
        private readonly TelemetryRepository telemetryRepository;
        private readonly AlarmRepository alarmRepository;
        private readonly DeviceRepository deviceRepository;

        public AlarmService(TelemetryRepository telemetryRepository, AlarmRepository alarmRepository)
        {
            this.telemetryRepository = telemetryRepository;
            this.alarmRepository = alarmRepository;
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
            // TODO : Add check on BatteryVoltage value
        }
        public Alarm CreateAlarm(string DeviceId, string SiteId, Alarm.Type AlarmType, Alarm.Gravity AlarmGravity, string ShortDescription, string Description)
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
            return alarm;
        }

    }
}
