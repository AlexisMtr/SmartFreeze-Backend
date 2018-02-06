using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFreeze.Profiles
{
    public class SiteRegistrationProfile : Profile
    {
        public SiteRegistrationProfile()
        {
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
