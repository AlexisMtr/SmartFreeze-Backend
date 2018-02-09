using Autofac;
using SmartFreezeScheduleFA.Repositories;
using SmartFreezeScheduleFA.Services;
using System.Configuration;
using WeatherLibrary.Abstraction;
using WeatherLibrary.Algorithmes.Freeze;
using WeatherLibrary.GoogleMapElevation;

namespace SmartFreezeScheduleFA.Configurations
{
    public static class ServiceLocator
    {
        public static IContainer Container { get; private set; }

        public static void ConfigureDI()
        {
            var builder = new ContainerBuilder();

            //DbContext
            builder.RegisterInstance(new DbContext(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString,
                ConfigurationManager.AppSettings["DefaultDbName"]))
                .InstancePerLifetimeScope();

            builder.RegisterType<CommunicationStateService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AlarmService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DeviceRepository>()
                .As<IDeviceRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GoogleMapElevationClient>()
                .As<IAltitudeClient>()
                .InstancePerLifetimeScope();
            builder.RegisterType<FreezingAlgorithme>()
                .As<IAlgorithme<FreezeForecast>>()
                .UsingConstructor(typeof(IAltitudeClient))
                .InstancePerLifetimeScope();

            Container = builder.Build();
        }
    }
}
