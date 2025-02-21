using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
