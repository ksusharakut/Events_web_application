using Application.UseCases.DTOs;

namespace Application.UseCases.Authorization.LogIn
{
    public interface ILogInParticipantUseCase
    {
        Task<AuthResultDTO> Handle(ParticipantLoginDTO request, CancellationToken cancellationToken);
    }
}
