using Application.UseCases.Authorization.LogIn;
using Application.UseCases.Authorization.RefreshToken;
using Application.UseCases.Authorization.Register;
using Application.UseCases.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterParticipantUseCase _registerParticipantUseCase;
        private readonly ILogInParticipantUseCase _logInUseCase;
        private readonly IRefreshTokenUseCase _refreshTokenUseCase;


        public AuthController(IRegisterParticipantUseCase registerParticipantUseCase, 
            ILogInParticipantUseCase logInUseCase, 
            IRefreshTokenUseCase refreshTokenUseCase)
        {
            _registerParticipantUseCase = registerParticipantUseCase;
            _logInUseCase = logInUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ParticipantRegistrationDTO request)
        {
            await _registerParticipantUseCase.ExecuteAsync(request, HttpContext.RequestAborted);

            return Ok("Registration successful");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(ParticipantLoginDTO loginDto)
        {
            var result = await _logInUseCase.ExecuteAsync(loginDto, CancellationToken.None);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _refreshTokenUseCase.ExecuteAsync(request, CancellationToken.None);
            return Ok(result);
        }
    }
}
