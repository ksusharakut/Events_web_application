using Application.Common;
using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces.RepositoryInterfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Get
{
    public class GetEventByTitleUseCase : IGetEventByTitleUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IImagePathService _imagePathService;
        private readonly IMapper _mapper;

        public GetEventByTitleUseCase(IEventRepository eventRepository, IMapper mapper, IImagePathService imagePathService)
        {
            _eventRepository = eventRepository;
            _imagePathService = imagePathService;
            _mapper = mapper;
        }

        public async Task<EventReturnDTO> ExecuteAsync(string title, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventRepository.GetByTitleAsync(title, cancellationToken);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Событие с названием '{title}' не найдено.");
            }

            var eventReturnDTO = _mapper.Map<EventReturnDTO>(eventEntity);

            eventReturnDTO.ImageUrl = _imagePathService.GetImageUrl(eventEntity.ImageUrl);

            return eventReturnDTO;
        }
    }
}
