using Application.Common;
using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation;

namespace Application.UseCases.Authorization.Register
{
    public class RegisterParticipantUseCase : IRegisterParticipantUseCase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<ParticipantRegistrationDTO> _validator;

        public RegisterParticipantUseCase(IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<ParticipantRegistrationDTO> validator)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task ExecuteAsync(ParticipantRegistrationDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingParticipant = await _unitOfWork.ParticipantRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingParticipant != null)
            {
                throw new ConflictException("Email is already in use.");
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
