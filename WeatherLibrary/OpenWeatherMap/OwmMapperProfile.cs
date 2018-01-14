using AutoMapper;
using WeatherLibrary.Extensions;
using WeatherLibrary.OpenWeatherMap.Internals;

namespace WeatherLibrary.OpenWeatherMap
{
    internal class OwmMapperProfile : Profile
    {
        public OwmMapperProfile()
        {
            CreateMap<OwmCurrentRoot, OwmWeather>()
                .ForMember(d => d.WindSpeed, opt => opt.MapFrom(s => s.Wind.Speed))
                .ForMember(d => d.Temperature, opt => opt.MapFrom(s => s.Weather.Temperature))
                .ForMember(d => d.Humidity, opt => opt.MapFrom(s => s.Weather.Humidity))
                .ForMember(d => d.Pressure, opt => opt.MapFrom(s => s.Weather.Pressure))
                .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Timestamp.ToDateTime()))
                .ForAllOtherMembers(cfg => cfg.Ignore());

            CreateMap<OwmCurrentRoot, OwmStationPosition>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Coord.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Coord.Longitude))
                .ForAllOtherMembers(cfg => cfg.Ignore());


            CreateMap<OwmForecastItem, OwmWeather>()
                .ForMember(d => d.WindSpeed, opt => opt.MapFrom(s => s.Wind.Speed))
                .ForMember(d => d.Temperature, opt => opt.MapFrom(s => s.Weather.Temperature))
                .ForMember(d => d.Pressure, opt => opt.MapFrom(s => s.Weather.Pressure))
                .ForMember(d => d.Humidity, opt => opt.MapFrom(s => s.Weather.Humidity))
                .ForMember(d => d.Date, opt => opt.MapFrom(s => s.ForecastDate))
                .ForAllOtherMembers(cfg => cfg.Ignore());

            CreateMap<OwmForecastRoot, OwmStationPosition>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.City.Coord.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.City.Coord.Longitude))
                .ForAllOtherMembers(cfg => cfg.Ignore());
        }
    }
}
