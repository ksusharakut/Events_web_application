using AutoMapper;
using Domain.Interfaces.RepositoryInterfaces;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Application.Common;
using Application.UseCases.DTOs;

namespace Application.UseCases.Events.Get
{
    public class GetAllEventsUseCase : IGetAllEventsUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IImagePathService _imagePathService;

        public GetAllEventsUseCase(IEventRepository eventRepository, IMapper mapper, IImagePathService imagePathService)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _imagePathService = imagePathService;
        }

        public async Task<IEnumerable<EventReturnDTO>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync(cancellationToken);
            var eventDtos = _mapper.Map<IEnumerable<EventReturnDTO>>(events);

            foreach (var eventDto in eventDtos)
            {
                eventDto.ImageUrl = _imagePathService.GetImageUrl(eventDto.ImageUrl);
            }

            return eventDtos;
        }
    }
}
