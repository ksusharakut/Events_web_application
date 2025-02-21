using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Participant.Get
{
    public class GetParticipantUseCase : IGetParticipantUseCase
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;

        public GetParticipantUseCase(IParticipantRepository participantRepository, IMapper mapper)
        {
            _participantRepository = participantRepository;
            _mapper = mapper;
        }

        public async Task<ParticipantReturnDTO> ExecuteAsync(int participantId, CancellationToken cancellationToken)
        {
            var participantEntity = await _participantRepository.GetByIdAsync(participantId, cancellationToken);
            if (participantEntity == null)
            {
                throw new KeyNotFoundException($"Участник с Id '{participantId}' не найден.");
            }

            return _mapper.Map<ParticipantReturnDTO>(participantEntity);
        }
    }
}
