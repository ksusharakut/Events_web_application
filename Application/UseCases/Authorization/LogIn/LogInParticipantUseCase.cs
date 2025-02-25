using Application.Common;
using Application.UseCases.DTOs;
using Domain.Interfaces;
using Domain.Exceptions;
using FluentValidation;

namespace Application.UseCases.Authorization.LogIn
{
    public class LogInParticipantUseCase : ILogInParticipantUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator<ParticipantLoginDTO> _validator;

        public LogInParticipantUseCase(IUnitOfWork unitOfWork, ITokenService tokenService,
            IPasswordHasher passwordHasher, IValidator<ParticipantLoginDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public async Task<AuthResultDTO> ExecuteAsync(ParticipantLoginDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var participant = await _unitOfWork.ParticipantRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (participant == null || !_passwordHasher.VerifyPassword(request.Password, participant.PasswordHash))
                throw new AuthenticationFailedException("Invalid email or password.");

            var accessToken = _tokenService.GenerateAccessToken(participant);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _tokenService.StoreRefreshToken(refreshToken, participant);

            return new AuthResultDTO { AccessToken = accessToken, RefreshToken = refreshToken };
        }
    }
}
