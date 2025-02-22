using Application.UseCases.DTOs;

namespace Application.UseCases.EventParticipant
{
    public interface IGetEventUseCase
    {
        Task<EventReturnDTO> ExecuteAsync(int eventId, CancellationToken cancellationToken);
    }
}