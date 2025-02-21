using AutoMapper;
using Application.Common;
using Application.UseCases.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Events.Get
{
    public class GetAllEventsUseCase : IGetAllEventsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImagePathService _imagePathService;

        public GetAllEventsUseCase(IUnitOfWork unitOfWork, IMapper mapper, IImagePathService imagePathService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imagePathService = imagePathService;
        }

        public async Task<IEnumerable<EventReturnDTO>> ExecuteAsync(CancellationToken cancellationToken, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and page size must be greater than 0.");
            }

            var events = await _unitOfWork.EventRepository.GetAllAsync(cancellationToken, pageNumber, pageSize);
            var eventDtos = _mapper.Map<IEnumerable<EventReturnDTO>>(events);

            foreach (var eventDto in eventDtos)
            {
                eventDto.ImageUrl = _imagePathService.GetImageUrl(eventDto.ImageUrl);
            }

            return eventDtos;
        }
    }
}
