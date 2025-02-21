using Application.UseCases.DTOs;

namespace Application.UseCases.Authorization.RefreshToken
{
    public interface IRefreshTokenUseCase
    {
        Task<AuthResultDTO> ExecuteAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
    }
}
