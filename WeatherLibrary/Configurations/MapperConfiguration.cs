using AutoMapper;
using AutoMapper.Configuration;
using WeatherLibrary.GoogleMapElevation;
using WeatherLibrary.OpenWeatherMap;

namespace WeatherLibrary.Configurations
{
    internal static class MapperConfiguration
    {
        private static bool isInitialized;

        internal static void ConfigureMapper()
        {
            if (isInitialized) return;

            var cfg = new MapperConfigurationExpression();

            cfg.AddProfile<OwmMapperProfile>();
            cfg.AddProfile<GmeMapperProfile>();

            Mapper.Initialize(cfg);
            isInitialized = true;
        }
    }
}
