using ProjetoBMA.DTOs;
using ProjetoBMA.Entities;
using AutoMapper;

namespace ProjetoBMA.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTimeEntryDto, TimeEntry>();
            CreateMap<UpdateTimeEntryDto, TimeEntry>();
            CreateMap<TimeEntry, TimeEntryDto>();
        }
    }
}
