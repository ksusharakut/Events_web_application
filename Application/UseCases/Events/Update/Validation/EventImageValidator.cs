using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Events.Update.Validation
{
    public class EventImageValidator : AbstractValidator<IFormFile>
    {
        public EventImageValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("File is required.")
                .Must(BeAValidFileType).WithMessage("Invalid file type. Allowed types are: jpg, jpeg, png, gif.")
                .Must(BeUnderMaxSize).WithMessage("File size exceeds the maximum limit of 5 MB.");
        }

        private bool BeAValidFileType(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
            return allowedExtensions.Contains(fileExtension);
        }

        private bool BeUnderMaxSize(IFormFile file)
        {
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB
            return file.Length <= maxFileSize;
        }
    }
}
