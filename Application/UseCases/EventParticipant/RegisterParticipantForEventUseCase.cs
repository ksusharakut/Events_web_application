using Application.Common;
using Application.UseCases.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.RepositoryInterfaces;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.EventParticipant
{
    public class RegisterParticipantForEventUseCase : IRegisterParticipantForEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService; // Сервис для получения текущего пользователя

        public RegisterParticipantForEventUseCase(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(RegisterParticipantDTO request, CancellationToken cancellationToken)
        {
            // Получаем текущего пользователя
            var currentUserId = _currentUserService.UserId;
            var currentUserRole = _currentUserService.Role;

            // Проверяем, что текущий пользователь имеет роль ParticipantOnly
            if (currentUserRole != "ParticipantOnly")
            {
                throw new UnauthorizedAccessException("Only users with the role 'ParticipantOnly' can register for events.");
            }

            // Проверяем, что событие существует
            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);
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

            // Проверяем, не зарегистрирован ли участник уже на это событие
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

            // Регистрация участника на событие
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