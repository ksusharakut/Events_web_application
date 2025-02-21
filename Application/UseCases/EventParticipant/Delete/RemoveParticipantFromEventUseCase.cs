using Application.Common;
using Domain.Interfaces;

namespace Application.UseCases.EventParticipant.Delete
{
    public class RemoveParticipantFromEventUseCase : IRemoveParticipantFromEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RemoveParticipantFromEventUseCase(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int eventId, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(eventId, cancellationToken);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            var participant = await _unitOfWork.ParticipantRepository.GetByIdAsync(currentUserId, cancellationToken);
            if (participant == null)
            {
                throw new KeyNotFoundException("Participant not found");
            }

            var participantEvent = await _unitOfWork.ParticipantEventRepository
                .GetByEventAndParticipantAsync(eventId, currentUserId, cancellationToken);

            if (participantEvent == null)
            {
                throw new InvalidOperationException("Participant is not registered for this event");
            }

            await _unitOfWork.ParticipantEventRepository.RemoveAsync(participantEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

}
