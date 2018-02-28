using Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using SmartFreezeFA.Configurations;
using SmartFreezeFA.Models;
using SmartFreezeFA.Parsers;
using SmartFreezeFA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherLibrary.Algorithmes.Freeze;

namespace SmartFreezeFA
{
    public static class TelemetryFunction
    {
        [FunctionName("TelemetryFunction")]
        public static async Task Run([EventHubTrigger("device-data", Connection = "EventHubConnectionString")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
            DependencyInjection.ConfigureInjection();
            IEnumerable<Telemetry> telemetries = null;

            try
            {
                telemetries = FrameParser.Parse(myEventHubMessage);
                if (!telemetries.Any()) return;
            }
            catch(FormatException e)
            {
                log.Error($"Error on parsing frame", e);
                return;
            }

            try
            {
                using (var scope = DependencyInjection.Container.BeginLifetimeScope())
                {
                    DeviceService deviceService = scope.Resolve<DeviceService>();
                    AlarmService alarmService = scope.Resolve<AlarmService>();
                    TelemetryService telemetryService = scope.Resolve<TelemetryService>();
                    FreezingAlgorithme algorithme = scope.Resolve<FreezingAlgorithme>();

                    telemetryService.InsertTelemetries(telemetries);
                    string siteId = deviceService.GetSiteId(telemetries.First().DeviceId);
                    log.Info($"Telemetries inserted");

                    Task<FreezeForecast> forecast = algorithme.Execute(telemetries.Last());
                    log.Info($"FreezeForecast executed");

                    log.Info($"Create humidity alarm ...");
                    alarmService.CreateHumidityAlarm(telemetries.Last(), siteId);
                    log.Info($"Create temperature alarm ...");
                    alarmService.CreateTemperatureAlarm(telemetries.Last(), siteId);
                    log.Info($"Create battery alarm ...");
                    alarmService.CreateBatteryAlarm(telemetries.Last(), siteId);

                    FreezeForecast freeze = await forecast;
                    if (freeze.FreezingStart.HasValue && Freeze(freeze.FreezingProbabilityList.FirstOrDefault().Value))
                    {
                        log.Info($"Create freeze alarm (if not already active)...");
                        alarmService.CreateFreezingAlarm(telemetries.Last(), siteId, freeze.FreezingStart, freeze.FreezingEnd);
                    }
                    else if(!Freeze(freeze.FreezingProbabilityList.FirstOrDefault().Value))
                    {
                        log.Info($"Set latest freeze alarm as inactive ...");
                        alarmService.SetFreezeAlarmAsInactive(telemetries.Last().DeviceId);
                    }

                    log.Info($"Remove CommunicationFail alarm ...");
                    alarmService.CheckForActiveCommunicationFailureAlarms(telemetries.First().DeviceId);

                    log.Info($"Update last communication date ...");
                    deviceService.UpdateLastCommunication(telemetries.Last().DeviceId, DateTime.UtcNow);
                }
            }
            catch(Exception e)
            {
                log.Error($"Error on processing telemetries", e);
                throw;
            }
        }

        private static bool Freeze(FreezeForecast.FreezingProbability probability)
        {
            return probability != FreezeForecast.FreezingProbability.ZERO && probability != FreezeForecast.FreezingProbability.MINIMUM;
        }
    }
}
