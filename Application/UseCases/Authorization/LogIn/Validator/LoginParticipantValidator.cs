using Application.UseCases.DTOs;
using FluentValidation;

namespace Application.UseCases.Authorization.LogIn.Validators
{
    public class LoginParticipantValidator : AbstractValidator<ParticipantLoginDTO>
    {
        public LoginParticipantValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(50).WithMessage("Password cannot exceed 50 characters.");
        }
    }
}
