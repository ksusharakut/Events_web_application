using Application.Common;
using Application.UseCases.DTOs;
using AutoMapper;
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
            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(eventId, cancellationToken);
            var eventDto = _mapper.Map<EventReturnDTO>(eventEntity);

            eventDto.ImageUrl = _imagePathService.GetImageUrl(eventDto.ImageUrl);

            return eventDto;
        }
    }
}
