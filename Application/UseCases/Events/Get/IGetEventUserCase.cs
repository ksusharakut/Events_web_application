using Application.UseCases.DTOs;

namespace Application.UseCases.Events.Get
{
    public interface IGetEventUseCase
    {
        Task<EventReturnDTO> ExecuteAsync(int eventId, CancellationToken cancellationToken);
    }
}