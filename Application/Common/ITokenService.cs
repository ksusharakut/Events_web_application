using Domain.Entities;

namespace Application.Common
{
    public interface ITokenService
    {
        string GenerateAccessToken(Participant participant);
        string GenerateRefreshToken();
        void StoreRefreshToken(string refreshToken, Participant participant);
        Participant GetParticipantByRefreshToken(string refreshToken);
        void RemoveRefreshToken(string refreshToken);
    }
}
