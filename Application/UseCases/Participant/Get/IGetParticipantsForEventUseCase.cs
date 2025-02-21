using Application.UseCases.DTOs;

namespace Application.UseCases.Participant.Get
{
    public interface IGetParticipantsForEventUseCase
    {
        Task<List<ParticipantReturnDTO>> ExecuteAsync(int eventId, CancellationToken cancellationToken);
    }
}
