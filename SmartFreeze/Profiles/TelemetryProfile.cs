using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Models;

namespace SmartFreeze.Profiles
{
    public class TelemetryProfile : Profile
    {
        public TelemetryProfile()
        {
            CreateMap<Telemetry, TelemetryDto>();
            CreateMap<Telemetry, TelemetryFahrenheitDto>()
                .ForMember(d => d.Temperature, opt => opt.MapFrom(s => ConvertToFahrenheit(s.Temperature)));
        }

        private double ConvertToFahrenheit(double temperature)
        {
            return temperature * 1.8 + 32;
        }
    }
}
