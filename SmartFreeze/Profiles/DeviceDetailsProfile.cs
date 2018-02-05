﻿using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Profiles
{
    public class DeviceDetailsProfile : Profile
    {
        public DeviceDetailsProfile()
        {
            CreateMap<Device, DeviceDetailsDto>()
                .ForMember(d => d.HasActiveAlarms, opt => opt.MapFrom(s => s.Alarms.Any(a => a.IsActive)))
                .ForMember(d => d.ActiveAlarmsCount, opt => opt.MapFrom(s => s.Alarms.Count()))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Position.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Position.Longitude));
        }
    }
}
