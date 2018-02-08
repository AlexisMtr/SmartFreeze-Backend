using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Models;

namespace SmartFreeze.Profiles
{
    public class SiteProfile : Profile
    {
        public SiteProfile()
        {
            CreateMap<Site, SiteDetailsDto>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Position.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Position.Longitude));

            CreateMap<Site, SiteOverviewDto>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Position.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Position.Longitude));

            CreateMap<PaginatedItems<Site>, PaginatedItemsDto<SiteOverviewDto>>();


            CreateMap<SiteOverviewDto, Site>()
                .ForMember(d => d.Position, map => map.MapFrom(s => new Position
                {
                    Longitude = s.Longitude,
                    Altitude = s.Latitude,
                    Latitude = s.Latitude,
                }));
        }
    }
}
