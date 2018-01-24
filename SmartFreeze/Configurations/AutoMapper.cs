using AutoMapper;
using AutoMapper.Configuration;

namespace SmartFreeze
{
    public partial class Startup
    {
        public void ConfigureMapper()
        {
            var configuration = new MapperConfigurationExpression();
            // TODO : Map profiles

            Mapper.Initialize(configuration);
        }
    }
}
