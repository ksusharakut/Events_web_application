using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mapping
{
    public class ParticipantMappingProfile : Profile
    {
        public ParticipantMappingProfile() 
        {
            CreateMap<ParticipantRegistrationDTO, Participant>();

            CreateMap<Participant, ParticipantReturnDTO>();
        }
    }
}
