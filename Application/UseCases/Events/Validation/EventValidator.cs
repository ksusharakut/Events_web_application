using FluentValidation;
using Application.UseCases.DTOs;
using Application.Common;


namespace Application.UseCases.Events.Validation
{
    public class EventValidator : AbstractValidator<EventDTO>
    {
        public EventValidator()
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

            When(e => e.ImageFile != null, () =>
            {
                RuleFor(e => e.ImageFile)
                    .Must(file => file.Length > 0).WithMessage("Image file cannot be empty.")
                    .Must(file => file.Length <= FileConstants.MaxFileSize).WithMessage($"File size must not exceed {FileConstants.MaxFileSize / 1024 / 1024} MB.")
                    .Must(file => FileConstants.AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                    .WithMessage($"Invalid file type. Allowed types: {string.Join(", ", FileConstants.AllowedExtensions)}.");
            });
        }
    }
}
