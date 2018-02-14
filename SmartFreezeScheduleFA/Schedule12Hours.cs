using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Services;
using WeatherLibrary.Algorithmes.Freeze;
using static WeatherLibrary.Algorithmes.Freeze.FreezeForecast;
using WeatherLibrary.OpenWeatherMap;

namespace SmartFreezeScheduleFA
{
    public static class Schedule12Hours
    {
        //"0 0 */12 * * *" timer
        [FunctionName("Schedule12Hours")]
        public static async Task Run([TimerTrigger("0 */3 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            DependencyInjection.ConfigureInjection();

            using (var scope = DependencyInjection.Container.BeginLifetimeScope())
            {
                AlarmService alarmService = scope.Resolve<AlarmService>();
                FreezingAlgorithme algorithme = scope.Resolve<FreezingAlgorithme>();
                DeviceService deviceService = scope.Resolve<DeviceService>();
                NotificationService notificationService = scope.Resolve<NotificationService>();
                FreezeService freezeService = scope.Resolve<FreezeService>();

                OpenWeatherMapClient weatherClient = scope.Resolve<OpenWeatherMapClient>();

                IList<Alarm> alarms = new List<Alarm>();

                Dictionary<Device, Telemetry> telemetries = deviceService.GetLatestTelemetryByDevice();
                foreach (var item in telemetries)
                {
                    OwmCurrentWeather current = await weatherClient.GetCurrentWeather(item.Key.Position.Latitude, item.Key.Position.Longitude);
                    OwmForecastWeather forecast = await weatherClient.GetForecastWeather(item.Key.Position.Latitude, item.Key.Position.Longitude);

                    FreezeForecast freeze = await algorithme.Execute(item.Value, item.Key, current.Weather, forecast.Forecast, forecast.StationPosition);

                    if (freeze.FreezingStart.HasValue) // TODO refactor le if car tj valeur de début
                    {
                        log.Info($"Create Alarm");
                        // TODO : complete process
                        Dictionary<DateTime, FreezingProbability> averageFreezePrediction12h = freezeService.CalculAverageFreezePrediction12h(freeze.FreezingProbabilityList);
                        freezeService.CreateFreezeAndThawByDevice(item.Key.Id, averageFreezePrediction12h);
                        alarmService.createFreezeAlarm(item.Value.DeviceId, item.Key.SiteId, averageFreezePrediction12h);
                        // - check gravity
                        // - check with Clarck possible values
                        //Alarm alarm = alarmService.CreateAlarm(item.Key.Id, item.Key.SiteId, Alarm.Type.FreezeWarning, Alarm.Gravity.Critical, string.Empty, string.Empty);
                        //alarms.Add(alarm);
                    }
                }

                notificationService.SendNotifications(alarms);
                log.Info($"Notifications sent at: {DateTime.Now}");
            }
        }
    }
}
