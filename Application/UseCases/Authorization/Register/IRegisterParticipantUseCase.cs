using Application.UseCases.DTOs;

namespace Application.UseCases.Authorization.Register
{
    public interface IRegisterParticipantUseCase
    {
        Task Handle(ParticipantRegistrationDTO request, CancellationToken cancellationToken);
    }
}
