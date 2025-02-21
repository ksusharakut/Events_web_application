using Application.UseCases.DTOs;

namespace Application.UseCases.Authorization.LogIn
{
    public interface ILogInParticipantUseCase
    {
        Task<AuthResultDTO> ExecuteAsync(ParticipantLoginDTO request, CancellationToken cancellationToken);
    }
}
