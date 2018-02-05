using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SmartFreeze.Configurations;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartFreeze
{
    public partial class Startup
    {
        public void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info
                {
                    Title = "SmartFreeze API",
                    Description = "",
                    Version = "1.0.0",
                    Contact = new Contact
                    {
                        Name = "AlexisMtr",
                        Url = "http://github.com/AlexisMtr"
                    }
                });
                config.OperationFilter<DefaultValueOperationFilter>();
                config.DescribeAllEnumsAsStrings();
            });
        }

        public void ConfigureSwaggerUI(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartFreeze API V1"));
        }
    }
}
