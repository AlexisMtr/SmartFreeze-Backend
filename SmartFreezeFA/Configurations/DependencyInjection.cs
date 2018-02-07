using Autofac;
using SmartFreezeFA.Repositories;
using SmartFreezeFA.Services;
using System.Configuration;

namespace SmartFreezeFA.Configurations
{
    public static class DependencyInjection
    {
        public static IContainer Container { get; private set; }

        public static void ConfigureInjection()
        {
            ContainerBuilder builder = new ContainerBuilder();

            DbContext context = new DbContext(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString, ConfigurationManager.AppSettings["DefaultDbName"]));
            builder.RegisterInstance(context);

            builder.RegisterType<AlarmRepository>();
            builder.RegisterType<TelemetryRepository>();

            builder.RegisterType<AlarmService>();

            Container = builder.Build();
        }
    }
}
