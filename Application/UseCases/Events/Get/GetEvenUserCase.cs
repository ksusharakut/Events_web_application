using Application.Common;
using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.UseCases.Events.Get
{
    public class GetEventUseCase : IGetEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImagePathService _imagePathService;

        public GetEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IImagePathService imagePathService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imagePathService = imagePathService;
        }

        public async Task<EventReturnDTO> ExecuteAsync(int eventId, CancellationToken cancellationToken)
        {
            if (eventId <= 0)
            {
                throw new ArgumentException("Invalid event ID.", nameof(eventId));
            }

            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(eventId, cancellationToken);

            if (eventEntity == null)
            {
                throw new NotFoundException($"Event with ID {eventId} not found.");
            }

            var eventDto = _mapper.Map<EventReturnDTO>(eventEntity);

            eventDto.ImageUrl = _imagePathService.GetImageUrl(eventDto.ImageUrl);

            return eventDto;
        }
    }
}
