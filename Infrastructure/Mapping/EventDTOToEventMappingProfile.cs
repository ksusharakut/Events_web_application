using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mapping
{
    public class EventDTOToEventMappingProfile : Profile
    {
        public EventDTOToEventMappingProfile()
        {
            CreateMap<EventDTO, Event>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }
    }
}
