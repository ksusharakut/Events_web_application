using Application.UseCases.DTOs;
using Domain.Entities;
using AutoMapper;

namespace Infrastructure.Mapping
{
    public class EventToEventReturnDTOMappingProfile : Profile
    {
        public EventToEventReturnDTOMappingProfile()
        {
            CreateMap<Event, EventReturnDTO>();
        }
    }
}
