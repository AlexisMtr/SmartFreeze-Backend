using Autofac;
using SmartFreezeFA.Repositories;
using SmartFreezeFA.Services;
using System.Configuration;
using WeatherLibrary.Abstraction;
using WeatherLibrary.Algorithmes.Freeze;
using WeatherLibrary.GoogleMapElevation;
using WeatherLibrary.OpenWeatherMap;

namespace SmartFreezeFA.Configurations
{
    public static class DependencyInjection
    {
        public static IContainer Container { get; private set; }

        public static void ConfigureInjection()
        {
            ContainerBuilder builder = new ContainerBuilder();

            DbContext context = new DbContext(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString, ConfigurationManager.AppSettings["DatabaseName"]);
            builder.RegisterInstance(context);
            
            builder.RegisterType<TelemetryRepository>()
                .As<ITelemetryRepository>();
            builder.RegisterType<DeviceRepository>()
                .As<IDeviceRepository>();
            builder.RegisterType<AlarmRepository>()
                .As<IAlarmRepository>();

            builder.RegisterType<TelemetryService>();
            builder.RegisterType<AlarmService>();
            builder.RegisterType<DeviceService>();
            
            builder.RegisterType<GoogleMapElevationClient>()
                .As<IAltitudeClient, GoogleMapElevationClient>()
                .UsingConstructor(typeof(string))
                .WithParameter("apiKey", ConfigurationManager.AppSettings["GmeApiKey"])
                .InstancePerLifetimeScope();

            builder.RegisterType<OpenWeatherMapClient>()
                .As<IWeatherClient<OwmCurrentWeather, OwmForecastWeather>, OpenWeatherMapClient>()
                .UsingConstructor(typeof(string), typeof(string), typeof(Unit))
                .WithParameter("apiKey", ConfigurationManager.AppSettings["OwmApiKey"])
                .WithParameter("unit", Unit.Metric)
                .InstancePerLifetimeScope();

            builder.RegisterType<FreezingAlgorithme>()
                .As<IAlgorithme<FreezeForecast>, FreezingAlgorithme>()
                .UsingConstructor(typeof(IAltitudeClient))
                .InstancePerLifetimeScope();

            Container = builder.Build();
        }
    }
}
