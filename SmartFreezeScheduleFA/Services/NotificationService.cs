using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreezeScheduleFA.Services
{
    public class NotificationService
    {
        private readonly IDeviceRepository deviceRepository;

        public NotificationService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public void SendNotifications(IEnumerable<Alarm> alarms)
        {
            IEnumerable<AlarmNotification> notificationsDetails = deviceRepository.GetNotificationDetails(alarms.Select(e => e.DeviceId));
            foreach(var alarm in alarms)
            {
                notificationsDetails.First(e => e.DeviceId == alarm.DeviceId).Alarm = alarm;
            }

            // TODO : Send notification through Hub
        }
    }
}
