using Microsoft.Extensions.DependencyInjection;
using SmartFreeze.Configurations;
using SmartFreeze.Context;

namespace SmartFreeze
{
    public partial class Startup
    {
        public void ConfigureDI(IServiceCollection services)
        {

        }

        public void ConfigureContext(IServiceCollection services)
        {
            services.Configure<ContextSettings>(Configuration.GetSection("ContextSettings"));
            services.AddScoped<SmartFreezeContext>();
        }
    }
}
