using Application.Common;
using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Get
{
    public class GetEventUseCase : IGetEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IImagePathService _imagePathService;

        public GetEventUseCase(IEventRepository eventRepository, IMapper mapper, IImagePathService imagePathService)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _imagePathService = imagePathService;
        }

        public async Task<EventReturnDTO> ExecuteAsync(int eventId, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId, cancellationToken);
            var eventDto = _mapper.Map<EventReturnDTO>(eventEntity);

            // Используем сервис для получения публичного пути к изображению
            eventDto.ImageUrl = _imagePathService.GetImageUrl(eventDto.ImageUrl);

            return eventDto;
        }
    }
}
