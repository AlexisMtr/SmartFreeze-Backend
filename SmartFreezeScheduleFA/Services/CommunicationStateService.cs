using SmartFreezeScheduleFA.Models;
using System.Collections.Generic;

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

        public void Run(int minHour, int maxHour, Alarm.Gravity gravity)
        {
            int minMin = minHour * 60 + 5;
            int minMax = maxHour * 60 + 5;

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
                    Alarm alarm = (device.Alarms as List<Alarm>).FindLast(e => e.AlarmGravity.Equals(gravity + 1) && e.IsActive);
                    alarm.IsActive = false;
                    alarmService.UpdateAlarm(device.Id, alarm);
                    alarmService.CreateCommunicationAlarm(device.Id, gravity);
                }
            }

            notificationService.SendNotifications(alarms);
        }
    }
}
