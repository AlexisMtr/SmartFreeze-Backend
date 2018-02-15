using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreeze.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<Device, DeviceDetailsDto>()
                .ForMember(d => d.HasActiveAlarms, opt => opt.MapFrom(s => s.Alarms.Any(a => a.IsActive)))
                .ForMember(d => d.ActiveAlarmsCount, opt => opt.MapFrom(s => s.Alarms.Count()))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Position.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Position.Longitude));

            CreateMap<Device, DeviceOverviewDto>()
                .ForMember(d => d.HasActiveAlarms, opt => opt.MapFrom(s => s.Alarms.Any(a => a.IsActive)))
                .ForMember(d => d.ActiveAlarmsCount, opt => opt.MapFrom(s => s.Alarms.Count()))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Position.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Position.Longitude));

            CreateMap<PaginatedItems<Device>, PaginatedItemsDto<DeviceOverviewDto>>();

            CreateMap<DeviceRegistrationDto, Device>()
                .ForMember(d => d.Alarms, opt => opt.UseValue(new List<Alarm>()))
                .ForMember(d => d.LastCommunication, opt => opt.UseValue(DateTime.UtcNow))
                .ForMember(d => d.Position, map => map.MapFrom(s => new Position
                {
                    Longitude = s.Longitude,
                    Altitude = s.Altitude,
                    Latitude = s.Latitude,
                }));
            
            CreateMap<DeviceUpdateDto, Device>()
                .ForMember(d => d.Position, map => map.MapFrom(s => new Position
                {
                    Longitude = s.Longitude,
                    Altitude = s.Latitude,
                    Latitude = s.Latitude,
                }));
        }
    }
}
