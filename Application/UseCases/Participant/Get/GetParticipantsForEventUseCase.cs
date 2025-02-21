using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Domain.Exceptions;

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
                throw new NotFoundException("Event not found");

            var participants = await _unitOfWork.ParticipantEventRepository.GetParticipantsByEventIdAsync(eventEntity, cancellationToken);
            return _mapper.Map<List<ParticipantReturnDTO>>(participants);
        }
    }
}
