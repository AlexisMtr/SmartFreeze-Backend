using AutoMapper;
using AutoMapper.Configuration;
using SmartFreeze.Profiles;

namespace SmartFreeze
{
    public partial class Startup
    {
        public void ConfigureMapper()
        {
            var configuration = new MapperConfigurationExpression();

            configuration.AddProfile<SiteProfile>();
            configuration.AddProfile<DeviceProfile>();
            configuration.AddProfile<AlarmProfile>();
            configuration.AddProfile<TelemetryProfile>();

            Mapper.Initialize(configuration);
        }
    }
}
