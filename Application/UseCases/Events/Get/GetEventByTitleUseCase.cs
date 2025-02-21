using Application.Common;
using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces;

namespace Application.UseCases.Events.Get
{
    public class GetEventByTitleUseCase : IGetEventByTitleUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImagePathService _imagePathService;
        private readonly IMapper _mapper;

        public GetEventByTitleUseCase(IUnitOfWork unitOfWork, IMapper mapper, IImagePathService imagePathService)
        {
            _unitOfWork = unitOfWork;
            _imagePathService = imagePathService;
            _mapper = mapper;
        }

        public async Task<EventReturnDTO> ExecuteAsync(string title, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetByTitleAsync(title, cancellationToken);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"No event with {title} title.");
            }

            var eventReturnDTO = _mapper.Map<EventReturnDTO>(eventEntity);

            eventReturnDTO.ImageUrl = _imagePathService.GetImageUrl(eventEntity.ImageUrl);

            return eventReturnDTO;
        }
    }
}
