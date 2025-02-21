using Application.Common;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public TokenService(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public string GenerateAccessToken(Participant participant)
        {
            Console.WriteLine($"Participant: Id={participant.Id}, Email={participant.Email}, Role={participant.Role}");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, participant.Id.ToString()),
                new Claim(ClaimTypes.Email, participant.Email),
                new Claim(ClaimTypes.Role, participant.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        public Participant GetParticipantByRefreshToken(string refreshToken)
        {
            return _cache.Get<Participant>(refreshToken); 
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            _cache.Remove(refreshToken); 
        }

        public void StoreRefreshToken(string refreshToken, Participant participant)
        {
            var expiryTime = DateTime.UtcNow.AddDays(7); 
            _cache.Set(refreshToken, participant, expiryTime); 
        }
    }
}
   