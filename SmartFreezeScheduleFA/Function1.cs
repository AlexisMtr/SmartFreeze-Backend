using System;
using Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Services;
using SmartFreezeScheduleFA.Models;
using System.Collections.Generic;
using WeatherLibrary.Algorithmes.Freeze;

namespace SmartFreezeScheduleFA
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run1([TimerTrigger("0 5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            ServiceLocator.ConfigureDI();

            using (var scope = ServiceLocator.Container.BeginLifetimeScope())
            {
                CommunicationStateService service = scope.Resolve<CommunicationStateService>();
                AlarmService alarmService= scope.Resolve<AlarmService>();
                int minMin = 1 * 60 + 5;
                int minMax = 2 * 60 + 5;
                IEnumerable<Device> devices = service.CheckDeviceCommunication(minMin, minMax);
                foreach (var device in devices)
                {
                    alarmService.CreateCommunicationAlarm(device.Id, Alarm.Gravity.Critical);
                }
            }

        }

        public static void Run4([TimerTrigger("0 */5 */4 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            ServiceLocator.ConfigureDI();

            using (var scope = ServiceLocator.Container.BeginLifetimeScope())
            {
                CommunicationStateService service = scope.Resolve<CommunicationStateService>();
                AlarmService alarmService = scope.Resolve<AlarmService>();
                int minMin = 4 * 60 + 5;
                int minMax = 5 * 60 + 5;
                IEnumerable<Device> devices = service.CheckDeviceCommunication(minMin, minMax);
                foreach (var device in devices)
                {
                    alarmService.CreateCommunicationAlarm(device.Id, Alarm.Gravity.Critical);
                }
            }

        }

        public static void Run7([TimerTrigger("0 */5 */7 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            ServiceLocator.ConfigureDI();

            using (var scope = ServiceLocator.Container.BeginLifetimeScope())
            {
                CommunicationStateService service = scope.Resolve<CommunicationStateService>();
                AlarmService alarmService = scope.Resolve<AlarmService>();
                int minMin = 7 * 60 + 5;
                int minMax = 8 * 60 + 5;
                IEnumerable<Device> devices = service.CheckDeviceCommunication(minMin, minMax);
                foreach(var device in devices)
                {
                    alarmService.CreateCommunicationAlarm(device.Id, Alarm.Gravity.Critical);
                }
            }

        }

        public static async void Run12([TimerTrigger("0 */0 */12 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            ServiceLocator.ConfigureDI();

            using (var scope = ServiceLocator.Container.BeginLifetimeScope())
            {
                AlarmService alarmService = scope.Resolve<AlarmService>();
                FreezingAlgorithme algorithme = scope.Resolve<FreezingAlgorithme>();

                Dictionary<string, Telemetry> devices = new Dictionary<string, Telemetry>();
                foreach(var item in devices)
                {
                    FreezeForecast freeze = await algorithme.Execute(item.Value);
                    if(freeze.FreezingStart.HasValue)
                    {
                        // TODO : complete process
                        //alarmService.CreateAlarm(item.Key, null, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, string.Empty, string.Empty);
                    }
                }
            }

        }
    }
}
