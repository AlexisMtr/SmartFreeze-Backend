using Autofac;
using Microsoft.Azure.WebJobs.Host;
using SmartFreezeScheduleFA.Repositories;
using SmartFreezeScheduleFA.Services;
using System.Configuration;
using WeatherLibrary.Abstraction;
using WeatherLibrary.Algorithmes.Freeze;
using WeatherLibrary.GoogleMapElevation;
using WeatherLibrary.OpenWeatherMap;

namespace SmartFreezeScheduleFA.Configurations
{
    public static class DependencyInjection
    {
        public static IContainer Container { get; private set; }

        public static void ConfigureInjection(TraceWriter logger)
        {
            var builder = new ContainerBuilder();

            //DbContext
            var context = new DbContext(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString,
                ConfigurationManager.AppSettings["DefaultDbName"]);

            builder.RegisterInstance(context)
                .SingleInstance();

            builder.Register(ctx => new Logger(logger)).As<ILogger, Logger>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommunicationStateService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AlarmService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<DeviceService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<NotificationService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TelemetryService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<FreezeService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DeviceRepository>()
                .As<IDeviceRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TelemetryRepository>()
                .As<ITelemetryRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<FreezeRepository>()
                .As<IFreezeRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OpenWeatherMapClient>()
                .As<IWeatherClient<OwmCurrentWeather, OwmForecastWeather>, OpenWeatherMapClient>()
                .UsingConstructor(typeof(string), typeof(string), typeof(Unit))
                .WithParameter("apiKey", ConfigurationManager.AppSettings["OwmApiKey"])
                .WithParameter("unit", Unit.Metric)
                .InstancePerLifetimeScope();
            
            builder.RegisterType<GoogleMapElevationClient>()
                .As<IAltitudeClient, GoogleMapElevationClient>()
                .UsingConstructor(typeof(string))
                .WithParameter("apiKey", ConfigurationManager.AppSettings["GmeApiKey"])
                .InstancePerLifetimeScope();

            builder.RegisterType<FreezingAlgorithme>()
                .As<IAlgorithme<FreezeForecast>, FreezingAlgorithme>()
                .UsingConstructor(typeof(IAltitudeClient), typeof(ILogger))
                .InstancePerLifetimeScope();

            Container = builder.Build();
        }
    }
}
