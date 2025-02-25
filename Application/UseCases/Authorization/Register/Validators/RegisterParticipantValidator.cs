using Application.Common;
using Application.UseCases.DTOs;
using FluentValidation;

namespace Application.UseCases.Authorization.Register.Validators
{
    public class RegisterParticipantValidator : AbstractValidator<ParticipantRegistrationDTO>
    {
        public RegisterParticipantValidator(IUserValidationService ageValidationService)
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters")
                .MaximumLength(30).WithMessage("Name cannot exceed 30 characters.");

            RuleFor(p => p.BirthDay)
                .NotEmpty().WithMessage("BirthDay is required.")
                .LessThan(DateTime.Today).WithMessage("Birthday must be in the past.")
                .Must(ageValidationService.IsUserOldEnough)
                .WithMessage("Participant must be at least 18 years old.");

            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.") 
            .EmailAddress().WithMessage("Invalid email address.") 
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(50).WithMessage("Password cannot exceed 50 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
        }

    }
}
