using Application.UseCases.DTOs;

namespace Application.UseCases.EventParticipant.Create
{
    public interface IRegisterParticipantForEventUseCase
    {
        Task ExecuteAsync(RegisterParticipantDTO request, CancellationToken cancellationToken);
    }
}
