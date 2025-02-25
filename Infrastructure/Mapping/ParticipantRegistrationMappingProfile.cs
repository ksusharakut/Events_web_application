using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mapping
{
    public class ParticipantRegistrationMappingProfile : Profile
    {
        public ParticipantRegistrationMappingProfile()
        {
            CreateMap<ParticipantRegistrationDTO, Participant>();
        }
    }
}
