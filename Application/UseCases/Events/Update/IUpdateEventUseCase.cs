using Application.UseCases.DTOs;

namespace Application.UseCases.Events.Update
{
    public interface IUpdateEventUseCase
    {
        Task ExecuteAsync(int id, EventDTO eventDto, CancellationToken cancellationToken);
    }
}
