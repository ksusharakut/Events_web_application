using Application.Common;
using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.RepositoryInterfaces;
using FluentValidation;

namespace Application.UseCases.Authorization.Register
{
    public class RegisterParticipantHandler : IRegisterParticipantUseCase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<ParticipantRegistrationDTO> _validator;

        public RegisterParticipantHandler(IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<ParticipantRegistrationDTO> validator)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task Handle(ParticipantRegistrationDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingParticipant = await _unitOfWork.AuthRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingParticipant != null)
            {
                throw new Exception("Email is already in use.");
            }

            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            var participant = _mapper.Map<Domain.Entities.Participant>(request);
            participant.PasswordHash= hashedPassword;
            participant.Role = Role.ParticipantOnly;

            await _unitOfWork.ParticipantRepository.AddAsync(participant, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken); 
        }
    }
}
