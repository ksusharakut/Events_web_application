using Application.UseCases.DTOs;

namespace Application.UseCases.Events.Create
{
    public interface ICreateEventUseCase
    {
        Task ExecuteAsync(EventDTO eventDto, CancellationToken cancellationToken);
    }
}