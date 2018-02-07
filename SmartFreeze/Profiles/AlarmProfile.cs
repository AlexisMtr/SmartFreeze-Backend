using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Models;

namespace SmartFreeze.Profiles
{
    public class AlarmProfile : Profile
    {
        public AlarmProfile()
        {
            CreateMap<Alarm, AlarmDetailsDto>();
        }
    }
}
