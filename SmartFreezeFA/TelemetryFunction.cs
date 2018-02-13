using Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using SmartFreezeFA.Configurations;
using SmartFreezeFA.Models;
using SmartFreezeFA.Parsers;
using SmartFreezeFA.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherLibrary.Algorithmes.Freeze;

namespace SmartFreezeFA
{
    public static class TelemetryFunction
    {
        [FunctionName("TelemetryFunction")]
        public static async void Run([EventHubTrigger("device-data", Connection = "EventHubConnectionString")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
            DependencyInjection.ConfigureInjection();

            IEnumerable<Telemetry> telemetries = FrameParser.Parse(myEventHubMessage);
            if (!telemetries.Any()) return;

            using (var scope = DependencyInjection.Container.BeginLifetimeScope())
            {
                AlarmService alarmService = scope.Resolve<AlarmService>();
                FreezingAlgorithme algorithme = scope.Resolve<FreezingAlgorithme>();

                Task<FreezeForecast> forecast = algorithme.Execute(telemetries.Last());

                foreach (Telemetry telemetry in telemetries)
                {
                    alarmService.CreateHumidityAlarm(telemetry);
                    alarmService.CreatePressureAlarm(telemetry);
                    alarmService.CreateTemperatureAlarm(telemetry);
                    alarmService.CreateBatteryAlarm(telemetry);
                }

                FreezeForecast freeze = await forecast;
                if (freeze.FreezingStart.HasValue)
                {
                    alarmService.CreateFreezingAlarm(telemetries.Last(), freeze.FreezingStart, freeze.FreezingEnd);
                }
            }
        }

        // METHODE DE TEST DU FA (appel http via postman)
        //[FunctionName("HttpFA")]
        //public static async void Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        //{
        //    log.Info($"C# Event Hub trigger function processed a message: {req}");
        //    DependencyInjection.ConfigureInjection();

        //    string body = await req.Content.ReadAsStringAsync();

        //    IEnumerable<Telemetry> telemetries = FrameParser.Parse(await req.Content.ReadAsStringAsync());

        //    using (var scope = DependencyInjection.Container.BeginLifetimeScope())
        //    {
        //        AlarmService alarmService = scope.Resolve<AlarmService>();
        //        FreezingAlgorithme algorithme = scope.Resolve<FreezingAlgorithme>();

        //        Task<FreezeForecast> forecast = algorithme.Execute(telemetries.Last());

        //        foreach (Telemetry telemetry in telemetries)
        //        {
        //            alarmService.CreateHumidityAlarm(telemetry);
        //            alarmService.CreatePressureAlarm(telemetry);
        //            alarmService.CreateTemperatureAlarm(telemetry);
        //            alarmService.CreateBatteryAlarm(telemetry);
        //        }

        //        FreezeForecast freeze = await forecast;
        //        if (freeze.FreezingStart.HasValue)
        //        {
        //            alarmService.CreateFreezingAlarm(telemetries.Last(), freeze.FreezingStart, freeze.FreezingEnd);
        //        }
        //    }
        //}
    }
}
