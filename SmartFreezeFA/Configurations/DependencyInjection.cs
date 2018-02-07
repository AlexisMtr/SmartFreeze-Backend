using Autofac;

namespace SmartFreezeFA.Configurations
{
    public static class DependencyInjection
    {
        public static IContainer Container { get; private set; }

        public static void ConfigureInjection()
        {
            ContainerBuilder builder = new ContainerBuilder();

            DbContext context = new DbContext("", "");
            builder.RegisterInstance(context);

            Container = builder.Build();
        }
    }
}
