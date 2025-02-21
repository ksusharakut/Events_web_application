using Application.Common;
using Application.Services;
using Application.UseCases.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Authorization.LogIn
{
    public class LogInParticipantHandler : ILogInParticipantUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator<ParticipantLoginDTO> _validator;

        public LogInParticipantHandler(IUnitOfWork unitOfWork, ITokenService tokenService,
            IPasswordHasher passwordHasher, IValidator<ParticipantLoginDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public async Task<AuthResultDTO> Handle(ParticipantLoginDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var participant = await _unitOfWork.AuthRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (participant == null || !_passwordHasher.VerifyPassword(request.Password, participant.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            var accessToken = _tokenService.GenerateAccessToken(participant);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _tokenService.StoreRefreshToken(refreshToken, participant);

            return new AuthResultDTO { AccessToken = accessToken, RefreshToken = refreshToken };
        }
    }
}
