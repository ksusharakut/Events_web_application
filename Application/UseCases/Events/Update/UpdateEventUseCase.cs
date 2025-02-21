using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces.RepositoryInterfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Update
{
    public class UpdateEventUseCase : IUpdateEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EventDTO> _eventUpdateValidator;

        public UpdateEventUseCase(IEventRepository eventRepository, IMapper mapper, IValidator<EventDTO> eventUpdateValidator)
        {
            _eventRepository = eventRepository;
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

            var existingEvent = await _eventRepository.GetByIdAsync(id, cancellationToken);
            if (existingEvent == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }

            _mapper.Map(eventDto, existingEvent);

            await _eventRepository.UpdateAsync(existingEvent, cancellationToken);
        }
    }
}
