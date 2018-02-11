using AutoMapper;
using System.Linq;
using WeatherLibrary.GoogleMapElevation.Internals;

namespace WeatherLibrary.GoogleMapElevation
{
    public class GmeMapperProfile : Profile
    {
        public GmeMapperProfile()
        {
            CreateMap<GMERoot, GmeElevation>()
                .ForMember(d => d.Altitude, opt => opt.MapFrom(s => s.Results.First().Elevation))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Results.First().Location.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Results.First().Location.Longitude));
        }
    }
}
