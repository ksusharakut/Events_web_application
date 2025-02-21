using Application.Common;
using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.RepositoryInterfaces;
using FluentValidation;
using System;

namespace Application.UseCases.Events.Create
{
    public class CreateEventHandler : ICreateEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<EventDTO> _validator;

        public CreateEventHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<EventDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }


        //TODO: нельзя создать 2 события с одинаковым названием 
        public async Task ExecuteAsync(EventDTO eventDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(eventDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var newEvent = _mapper.Map<Event>(eventDto);

            await _unitOfWork.EventRepository.AddAsync(newEvent, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
