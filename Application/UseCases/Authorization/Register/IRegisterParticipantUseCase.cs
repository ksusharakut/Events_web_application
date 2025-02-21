using Application.UseCases.DTOs;

namespace Application.UseCases.Authorization.Register
{
    public interface IRegisterParticipantUseCase
    {
        Task ExecuteAsync(ParticipantRegistrationDTO request, CancellationToken cancellationToken);
    }
}
