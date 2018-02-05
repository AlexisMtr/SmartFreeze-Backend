using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Profiles
{
    public class DeviceOrverviewProfile : Profile
    {
        public DeviceOrverviewProfile()
        {
            CreateMap<Device, DeviceOverviewDto>()
                .ForMember(d => d.HasActiveAlarms, opt => opt.MapFrom(s => s.Alarms.Any(a => a.IsActive)))
                .ForMember(d => d.ActiveAlarmsCount, opt => opt.MapFrom(s => s.Alarms.Count()));
        }
    }
}
