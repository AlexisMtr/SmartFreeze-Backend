﻿using SmartFreezeScheduleFA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherLibrary.Abstraction;

namespace SmartFreezeScheduleFA.Services
{
    public class CommunicationStateService
    {
        private readonly DeviceService deviceService;
        private readonly AlarmService alarmService;
        private readonly NotificationService notificationService;
        private readonly ILogger logger;

        public CommunicationStateService(DeviceService deviceService, AlarmService alarmService, NotificationService notificationService, ILogger logger)
        {
            this.deviceService = deviceService;
            this.alarmService = alarmService;
            this.notificationService = notificationService;
            this.logger = logger;
        }

        public void Run(int minHour, int? maxHour, Alarm.Gravity gravity)
        {
            try
            {
                int minMin = minHour * 60 + 5;
                int? minMax = maxHour.HasValue ? (maxHour.Value * 60 + 5) : (int?)null;

                IEnumerable<Device> devices = deviceService.CheckDeviceCommunication(minMin, minMax);
                IList<Alarm> alarms = new List<Alarm>();

                logger.Info($"{devices.Count()} device(s) which has Communication Alarms");

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
                            .Where(e => (int)e.AlarmGravity == ((int)gravity + 1) && e.IsActive)
                            .OrderBy(e => e.OccuredAt).LastOrDefault();

                        if (alarm != null)
                        {
                            alarm.IsActive = false;
                            alarmService.UpdateAlarm(device.Id, alarm);
                        }

                        alarmService.CreateCommunicationAlarm(device.Id, device.SiteId, device.LastCommunication, gravity);
                    }
                }

                notificationService.SendNotifications(alarms);
            }
            catch(Exception e)
            {
                logger.Error($"Error on create communication alarms", e);
            }
        }
    }
}
