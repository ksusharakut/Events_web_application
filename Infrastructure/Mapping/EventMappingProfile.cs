using AutoMapper;
using Domain.Entities;
using System;
using Application.UseCases.DTOs;

namespace Infrastructure.Mapping
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<EventDTO, Event>();

            CreateMap<Event, EventReturnDTO>();

        }
    }
}
