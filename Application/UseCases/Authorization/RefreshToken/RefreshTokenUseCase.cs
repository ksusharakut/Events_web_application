using Application.Common;
using Domain.Exceptions;
using Application.UseCases.DTOs;

namespace Application.UseCases.Authorization.RefreshToken
{
    public class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenUseCase(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<AuthResultDTO> ExecuteAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var refreshToken = request.RefreshToken;
            var participant = _tokenService.GetParticipantByRefreshToken(refreshToken);
            if (participant == null)
            {
                throw new InvalidTokenException("Invalid or expired refresh token.");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(participant);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _tokenService.RemoveRefreshToken(refreshToken);
            _tokenService.StoreRefreshToken(newRefreshToken, participant);

            return new AuthResultDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
        }
    }
}
