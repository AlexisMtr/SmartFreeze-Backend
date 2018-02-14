using Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using SmartFreezeFA.Configurations;
using SmartFreezeFA.Models;
using SmartFreezeFA.Parsers;
using SmartFreezeFA.Services;
using System.Collections.Generic;

namespace SmartFreezeFA
{
    public static class TelemetryFunction
    {
        [FunctionName("TelemetryFunction")]
        public static void Run([EventHubTrigger("device-data", Connection = "EventHubConnectionString")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
            DependencyInjection.ConfigureInjection();

            IEnumerable<Telemetry> telemetries = FrameParser.Parse(myEventHubMessage);

            using (var scope = DependencyInjection.Container.BeginLifetimeScope())
            {
                AlarmService alarmService = scope.Resolve<AlarmService>();

                foreach(Telemetry telemetry in telemetries)
                {
                    alarmService.CreateHumidityAlarm(telemetry);
                    alarmService.CreatePressureAlarm(telemetry);
                    alarmService.CreateTemperatureAlarm(telemetry);
                    alarmService.CreateBatteryAlarm(telemetry);
                }
            }
        }
    }
}
