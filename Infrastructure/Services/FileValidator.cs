using Application.Common;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class FileValidator : IFileValidator
    {
        public bool BeAValidFileType(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
            return FileConstants.AllowedExtensions.Contains(fileExtension);
        }

        public bool BeUnderMaxSize(IFormFile file)
        {
            return file.Length <= FileConstants.MaxFileSize;
        }
    }
}
