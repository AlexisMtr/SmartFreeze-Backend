﻿using Autofac;
using SmartFreezeScheduleFA.Services;
using System.Configuration;

namespace SmartFreezeScheduleFA.Configurations
{
    public static class ServiceLocator
    {
        public static IContainer Container { get; private set; }

        public static void ConfigureDI()
        {
            var builder = new ContainerBuilder();

            //DbContext
            builder.RegisterInstance(new DbContext(ConfigurationManager.AppSettings["DefaultConnectionString"], ConfigurationManager.AppSettings["DefaultDbName"]));
            builder.RegisterType<CommunicationStateService>().InstancePerLifetimeScope();
            builder.RegisterType<AlarmService>().InstancePerLifetimeScope();

            Container = builder.Build();
        }
    }
}
