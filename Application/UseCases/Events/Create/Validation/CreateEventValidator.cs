using FluentValidation;
using Application.UseCases.DTOs;


namespace Application.UseCases.Events.Create.Validation
{
    public class CreateEventValidator : AbstractValidator<EventDTO>
    {
        public CreateEventValidator()
        {
            RuleFor(e => e.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(e => e.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(e => e.DateTime)
                .GreaterThan(DateTime.UtcNow).WithMessage("Event date must be in the future.");

            RuleFor(e => e.Location)
                .NotEmpty().WithMessage("Location is required.");

            RuleFor(e => e.Category)
                .NotEmpty().WithMessage("Category is required.");

            RuleFor(e => e.MaxParticipants)
                .GreaterThan(0).WithMessage("Max participants must be greater than zero.");
        }
    }
}
