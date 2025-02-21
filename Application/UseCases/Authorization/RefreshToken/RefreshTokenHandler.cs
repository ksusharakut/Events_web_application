using Application.Common;
using Application.UseCases.DTOs;

namespace Application.UseCases.Authorization.RefreshToken
{
    public class RefreshTokenHandler : IRefreshTokenUseCase
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<AuthResultDTO> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var refreshToken = request.RefreshToken;
            var participant = _tokenService.GetParticipantByRefreshToken(refreshToken);
            if (participant == null)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(participant);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _tokenService.RemoveRefreshToken(refreshToken);
            _tokenService.StoreRefreshToken(newRefreshToken, participant);

            return new AuthResultDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
        }
    }
}
