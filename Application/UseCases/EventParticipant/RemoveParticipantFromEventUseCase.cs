using Application.Common;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EventParticipant
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
            // Получаем текущего пользователя
            var currentUserId = _currentUserService.UserId;

            // Проверяем, что событие существует
            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(eventId, cancellationToken);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            // Проверяем, что участник существует
            var participant = await _unitOfWork.ParticipantRepository.GetByIdAsync(currentUserId, cancellationToken);
            if (participant == null)
            {
                throw new KeyNotFoundException("Participant not found");
            }

            // Проверка, что запись о связи между событием и участником существует
            var participantEvent = await _unitOfWork.ParticipantEventRepository
                .GetByEventAndParticipantAsync(eventId, currentUserId, cancellationToken);

            if (participantEvent == null)
            {
                throw new InvalidOperationException("Participant is not registered for this event");
            }

            // Удаление связи
            await _unitOfWork.ParticipantEventRepository.RemoveAsync(participantEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

}
