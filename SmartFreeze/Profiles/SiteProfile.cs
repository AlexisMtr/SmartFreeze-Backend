﻿using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Models;
using System.Linq;

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
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Position.Longitude))
                .ForMember(d => d.HasActiveAlarms, opt => opt.MapFrom(s => s.Devices.Any(d => d.Alarms.Any(a => a.IsActive))))
                .ForMember(d => d.ActiveAlarmsCount, opt => opt.MapFrom(s => s.Devices.SelectMany(d => d.Alarms).Count()));

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
