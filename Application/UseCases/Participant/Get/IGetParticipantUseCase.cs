using Application.UseCases.DTOs;

namespace Application.UseCases.Participant.Get
{
    public interface IGetParticipantUseCase
    {
        Task<ParticipantReturnDTO> ExecuteAsync(int participantId, CancellationToken cancellationToken);
    }
}
