using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Profiles
{
    public class SiteOverviewProfile : Profile
    {
        public SiteOverviewProfile()
        {
            CreateMap<Site, SiteOverviewDto>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Position.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Position.Longitude))
                .ForMember(d => d.HasActiveAlarms, opt => opt.MapFrom(s => s.Alarms.Any(a => a.IsActive)));

            CreateMap<PaginatedItems<Site>, PaginatedItemsDto<SiteOverviewDto>>();
        }
    }
}
