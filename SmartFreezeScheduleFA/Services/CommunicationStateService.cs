using SmartFreezeScheduleFA.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreezeScheduleFA.Services
{
    public class CommunicationStateService
    {
        private readonly DeviceService deviceService;
        private readonly AlarmService alarmService;
        private readonly NotificationService notificationService;

        public CommunicationStateService(DeviceService deviceService, AlarmService alarmService, NotificationService notificationService)
        {
            this.deviceService = deviceService;
            this.alarmService = alarmService;
            this.notificationService = notificationService;
        }

        public void Run(int minHour, int? maxHour, Alarm.Gravity gravity)
        {
            int minMin = minHour * 60 + 5;
            int? minMax = maxHour.HasValue ? (maxHour.Value * 60 + 5) : (int?)null;

            IEnumerable<Device> devices = deviceService.CheckDeviceCommunication(minMin, minMax);
            IList<Alarm> alarms = new List<Alarm>();

            foreach (var device in devices)
            {
                bool createAlarm = true;
                foreach (Alarm alarm in device.Alarms)
                {
                    if (alarm.AlarmType.Equals(Alarm.Type.CommuniationFailure)
                        && alarm.AlarmGravity.Equals(gravity)
                        && alarm.IsActive)
                    {
                        createAlarm = false;
                    }


                }
                if (createAlarm)
                {
                    Alarm alarm = device.Alarms
                        .Where(e => e.AlarmType == Alarm.Type.CommuniationFailure)
                        .Where(e => (int)e.AlarmGravity < (int)gravity && e.IsActive)
                        .OrderBy(e => e.OccuredAt).LastOrDefault();

                    if(alarm != null)
                    {
                        alarm.IsActive = false;
                        alarmService.UpdateAlarm(device.Id, alarm);
                    }

                    alarmService.CreateCommunicationAlarm(device.Id, device.SiteId, device.LastCommunication, gravity);
                }
            }

            notificationService.SendNotifications(alarms);
        }
    }
}
