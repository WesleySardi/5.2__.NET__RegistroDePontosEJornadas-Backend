using AutoMapper;
using ProjetoBMA.Domain.Entities;
using ProjetoBMA.DTOs.Commands;
using ProjetoBMA.DTOs.Results;
using ProjetoBMA.DTOs.ViewModels;

namespace ProjetoBMA.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TimeEntry, TimeEntryViewModel>();
            CreateMap<CreateTimeEntryCommand, TimeEntry>();
            CreateMap<UpdateTimeEntryCommand, TimeEntry>();
            CreateMap<TimeEntry, TimeEntryResult>();
        }
    }
}
