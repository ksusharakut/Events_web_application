using Application.Common;
using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.UseCases.Events.Create
{
    public class CreateEventUseCase : ICreateEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<EventDTO> _validator;
        private readonly IFileService _fileService;
        private readonly string _imageStoragePath = "wwwroot/images/events";

        public CreateEventUseCase(IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<EventDTO> validator,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _fileService = fileService;
        }

        public async Task ExecuteAsync(EventDTO eventDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(eventDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var newEvent = _mapper.Map<Event>(eventDto);

            if (eventDto.ImageFile != null)
            {
                string fileExtension = Path.GetExtension(eventDto.ImageFile.FileName).ToLower();
                string fileName = $"{Guid.NewGuid()}{fileExtension}"; 
                string filePath = await _fileService.SaveFileAsync(eventDto.ImageFile, fileName, _imageStoragePath, cancellationToken);

                newEvent.ImageUrl = filePath;
            }

            await _unitOfWork.EventRepository.AddAsync(newEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
