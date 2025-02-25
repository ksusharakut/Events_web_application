using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Domain.Exceptions;
using Application.Common;

namespace Application.UseCases.Events.Update
{
    public class UpdateEventUseCase : IUpdateEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<EventDTO> _validator;
        private readonly IFileService _fileService;
        private readonly string _imageStoragePath = "wwwroot/images/events";

        public UpdateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<EventDTO> eventUpdateValidator, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = eventUpdateValidator;
            _fileService = fileService;
        }

        public async Task ExecuteAsync(int id,EventDTO eventDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(eventDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(id, cancellationToken);
            if (existingEvent == null)
            {
                throw new NotFoundException($"Event with ID {id} not found.");
            }

            if (eventDto.ImageFile != null && !string.IsNullOrEmpty(existingEvent.ImageUrl))
            {
                await _fileService.DeleteFileAsync(existingEvent.ImageUrl, cancellationToken);
            }

            if (eventDto.ImageFile != null)
            {
                string fileExtension = Path.GetExtension(eventDto.ImageFile.FileName).ToLower();
                string fileName = $"{Guid.NewGuid()}{fileExtension}"; 
                string filePath = await _fileService.SaveFileAsync(eventDto.ImageFile, fileName, _imageStoragePath, cancellationToken);

                existingEvent.ImageUrl = filePath;
            }

            _mapper.Map(eventDto, existingEvent);

            await _unitOfWork.EventRepository.UpdateAsync(existingEvent, cancellationToken);
        }
    }
}
