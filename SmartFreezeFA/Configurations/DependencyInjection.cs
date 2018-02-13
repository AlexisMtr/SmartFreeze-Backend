using Autofac;
using SmartFreezeFA.Repositories;
using SmartFreezeFA.Services;
using System.Configuration;
using WeatherLibrary.Abstraction;
using WeatherLibrary.Algorithmes.Freeze;
using WeatherLibrary.GoogleMapElevation;

namespace SmartFreezeFA.Configurations
{
    public static class DependencyInjection
    {
        public static IContainer Container { get; private set; }

        public static void ConfigureInjection()
        {
            ContainerBuilder builder = new ContainerBuilder();

            DbContext context = new DbContext(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString, ConfigurationManager.AppSettings["DefaultDbName"]);
            builder.RegisterInstance(context);
            
            builder.RegisterType<TelemetryRepository>()
                .As<ITelemetryRepository>();
            builder.RegisterType<DeviceRepository>()
                .As<IDeviceRepository>();

            builder.RegisterType<AlarmService>();

            builder.RegisterType<FreezingAlgorithme>().As<IAlgorithme<FreezeForecast>, FreezingAlgorithme>();

            builder.RegisterType<GoogleMapElevationClient>()
                .As<IAltitudeClient>();
            builder.RegisterType<FreezingAlgorithme>()
                .As<IAlgorithme<FreezeForecast>>()
                .UsingConstructor(typeof(IAltitudeClient));

            Container = builder.Build();
        }
    }
}
