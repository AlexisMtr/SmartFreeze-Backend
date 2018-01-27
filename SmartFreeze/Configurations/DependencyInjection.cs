using Microsoft.Extensions.DependencyInjection;
using SmartFreeze.Configurations;
using SmartFreeze.Context;
using SmartFreeze.Repositories;
using SmartFreeze.Services;

namespace SmartFreeze
{
    public partial class Startup
    {
        public void ConfigureDI(IServiceCollection services)
        {
            services.AddScoped<DeviceRepository>();
            services.AddScoped<TelemetryRepository>();

            services.AddScoped<DeviceService>();
            services.AddScoped<TelemetryService>();
        }

        public void ConfigureContext(IServiceCollection services)
        {
            services.Configure<ContextSettings>(Configuration.GetSection("ContextSettings"));
            services.AddScoped<SmartFreezeContext>();
        }
    }
}
