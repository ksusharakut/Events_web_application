using Domain.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Delete
{
    public class DeleteEventUseCase : IDeleteEventUseCase
    {
        private readonly IEventRepository _eventRepository;

        public DeleteEventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        //TODO: отредачить всё в одном стиле

        public async Task ExecuteAsync(int eventId, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId, cancellationToken);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Событие с ID {eventId} не найдено.");
            }

            await _eventRepository.DeleteAsync(eventEntity, cancellationToken);
        }
    }
}
