using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Participant.Get
{
    public class GetParticipantsForEventUseCase : IGetParticipantsForEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetParticipantsForEventUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ParticipantReturnDTO>> ExecuteAsync(int eventId, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(eventId, cancellationToken);
            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found");

            var participants = await _unitOfWork.ParticipantEventRepository.GetParticipantsByEventIdAsync(eventEntity, cancellationToken);
            return _mapper.Map<List<ParticipantReturnDTO>>(participants);
        }
    }
}
