using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SmartFreeze
{
    public partial class Startup
    {
        public void ConfigureCors(IServiceCollection services)
        {
            services.AddCors();
        }

        public void ConfigureCorsPolicy(IApplicationBuilder app)
        {
            app.UseCors(builder => builder.AllowAnyOrigin());
        }
    }
}
