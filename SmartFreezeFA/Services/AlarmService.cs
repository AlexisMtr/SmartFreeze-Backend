using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartFreezeFA.Models;
using SmartFreezeFA.Repositories;

namespace SmartFreezeFA.Services
{
    public class AlarmService
    {
        private readonly DeviceRepository deviceRepository;
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
