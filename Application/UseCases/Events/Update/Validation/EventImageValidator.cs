using Application.Common;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Events.Update.Validation
{
    public class EventImageValidator : AbstractValidator<IFormFile>
    {
        private readonly IFileValidator _fileValidator;

        public EventImageValidator(IFileValidator fileValidator)
        {
            _fileValidator = fileValidator;

            RuleFor(x => x)
                .NotNull().WithMessage("File is required.")
                .Must(file => _fileValidator.BeAValidFileType(file)).WithMessage("Invalid file type. Allowed types are: jpg, jpeg, png, gif.")
                .Must(file => _fileValidator.BeUnderMaxSize(file)).WithMessage("File size exceeds the maximum limit of 5 MB.");
        }

    }
}
