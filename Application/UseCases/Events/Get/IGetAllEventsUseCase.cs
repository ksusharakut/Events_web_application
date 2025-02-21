using Application.UseCases.DTOs;

namespace Application.UseCases.Events.Get
{
    public interface IGetAllEventsUseCase
    {
        Task<IEnumerable<EventReturnDTO>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
