using Application.Common;
using Application.UseCases.DTOs;
using Application.UseCases.EventParticipant.Create;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.EventParticipant
{
    public class RegisterParticipantForEventUseCase : IRegisterParticipantForEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService; 

        public RegisterParticipantForEventUseCase(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(RegisterParticipantDTO request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var currentUserRole = _currentUserService.Role;

            if (currentUserRole != "ParticipantOnly")
            {
                throw new UnauthorizedAccessException("Only users with the role 'ParticipantOnly' can register for events.");
            }

            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            var participant = await _unitOfWork.ParticipantRepository.GetByIdAsync(currentUserId, cancellationToken);
            if (participant == null)
            {
                throw new KeyNotFoundException("Participant not found");
            }

            var existingRegistration = await _unitOfWork.ParticipantEventRepository
                .GetByEventAndParticipantAsync(eventEntity.Id, participant.Id, cancellationToken);

            if (existingRegistration != null)
            {
                throw new InvalidOperationException("Participant is already registered for this event.");
            }

            var currentParticipantsCount = await _unitOfWork.ParticipantEventRepository
        .GetParticipantsCountByEventAsync(eventEntity.Id, cancellationToken);

            if (currentParticipantsCount >= eventEntity.MaxParticipants)
            {
                throw new InvalidOperationException("The event has reached the maximum number of participants.");
            }

            var participantEvent = new ParticipantEvent
            {
                EventId = eventEntity.Id,
                ParticipantId = participant.Id
            };

            await _unitOfWork.ParticipantEventRepository.RegisterAsync(participantEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}