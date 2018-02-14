using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Models;

namespace SmartFreeze.Profiles
{
    public class AlarmProfile : Profile
    {
        public AlarmProfile()
        {
            CreateMap<Alarm, AlarmDetailsDto>()
                .ForMember(d => d.Gravity, opt => opt.MapFrom(s => (int)s.AlarmGravity))
                .ForMember(d => d.Type, opt => opt.MapFrom(s => (int)s.AlarmType));
            CreateMap<PaginatedItems<Alarm>, PaginatedItemsDto<AlarmDetailsDto>>();
        }
    }
}
