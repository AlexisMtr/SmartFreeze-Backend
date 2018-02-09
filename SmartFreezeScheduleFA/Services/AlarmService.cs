using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;

namespace SmartFreezeScheduleFA.Services
{
    public class AlarmService
    {
        private readonly IDeviceRepository deviceRepository;

        public AlarmService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public Alarm CreateAlarm(string deviceId, string siteId, Alarm.Type alarmType, Alarm.Gravity alarmGravity, string shortDescription, string description)
        {
            Alarm alarm = new Alarm()
            {
                Id = DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
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
            
        public void CreateAlarms (IEnumerable<Device> devices, Alarm.Gravity gravity, Alarm.Type type)
        {
            (string shortDesc, string desc) = SetGravityCommunicationDescription(gravity);
            foreach (var device in devices)
            {
                CreateAlarm(device.Id, null, type, gravity, shortDesc, desc);
            }
        }

        public (string shortDesc, string desc) SetGravityCommunicationDescription(Alarm.Gravity gravity)
        {
            String Desc = "";
            String ShortDesc = "";

            switch (gravity)
            {
                case Alarm.Gravity.Information:
                    {
                        Desc = "Pas de reception de mesures depuis plus d'une heure (entre 1 et 2 heures)";
                        ShortDesc = "echec communication 1h";
                    }
                    break;
                case Alarm.Gravity.Serious:
                    {
                        Desc = "Pas de reception de mesures depuis plus de 4 heures (entre 4 et 5 heures)";
                        ShortDesc = "echec communication 4h";
                    }
                    break;
                case Alarm.Gravity.Critical:
                    {
                        Desc = "Pas de reception de mesures depuis plus de 7 heures (entre 7 et 8 heures)";
                        ShortDesc = "echec communication 7h";
                    }
                    break;

            }
            return (ShortDesc, Desc);
        }

    }
}
