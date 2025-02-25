using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mapping
{
    public class ParticipantReturnMappingProfile : Profile
    {
        public ParticipantReturnMappingProfile()
        {
            CreateMap<Participant, ParticipantReturnDTO>();
        }
    }
}
