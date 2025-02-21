using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;

namespace Application.UseCases.Events.Update
{
    public class UpdateEventUseCase : IUpdateEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<EventDTO> _eventUpdateValidator;

        public UpdateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<EventDTO> eventUpdateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _eventUpdateValidator = eventUpdateValidator;
        }

        public async Task ExecuteAsync(int id,EventDTO eventDto, CancellationToken cancellationToken)
        {
            var validationResult = await _eventUpdateValidator.ValidateAsync(eventDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(id, cancellationToken);
            if (existingEvent == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }

            _mapper.Map(eventDto, existingEvent);

            await _unitOfWork.EventRepository.UpdateAsync(existingEvent, cancellationToken);
        }
    }
}
