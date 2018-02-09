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

        public void CreateCommunicationAlarm(string deviceId, Alarm.Gravity gravity)
        {
            String Desc = "";
            String ShortDesc = "";

            switch (gravity)
            {
                case Alarm.Gravity.Information:
                    {
                        Desc = "Pas de reception de mesures depuis plus d'une heure (entre 1 et 2 heures)";
                        ShortDesc = "echec communication 1h";
                        CreateAlarm(deviceId, null, Alarm.Type.CommunicationError, gravity, ShortDesc, Desc);
                    }
                    break;
                case Alarm.Gravity.Serious:
                    {
                        Desc = "Pas de reception de mesures depuis plus de 4 heures (entre 4 et 5 heures)";
                        ShortDesc = "echec communication 4h";
                        CreateAlarm(deviceId, null, Alarm.Type.CommunicationError, gravity, ShortDesc, Desc);
                    }
                    break;
                case Alarm.Gravity.Critical:
                    {
                        Desc = "Pas de reception de mesures depuis plus de 7 heures (entre 7 et 8 heures)";
                        ShortDesc = "echec communication 7h";
                        CreateAlarm(deviceId, null, Alarm.Type.CommunicationError, gravity, ShortDesc, Desc);
                    }
                    break;
            }
        }

        public void CreateAlarm(string DeviceId, string SiteId, Alarm.Type AlarmType, Alarm.Gravity AlarmGravity, string ShortDescription, string Description)
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
