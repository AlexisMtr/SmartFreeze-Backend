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
            services.AddScoped<SiteRepository>();
            services.AddScoped<AlarmRepository>();
            services.AddScoped<IFreezeRepository, FreezeRepository>();

            services.AddScoped<DeviceService>();
            services.AddScoped<TelemetryService>();
            services.AddScoped<SiteService>();
            services.AddScoped<AlarmService>();
            services.AddScoped<FreezeService>();
        }

        public void ConfigureContext(IServiceCollection services)
        {
            services.Configure<ContextSettings>(Configuration.GetSection("ContextSettings"));
            services.AddScoped<SmartFreezeContext>();
        }
    }
}
