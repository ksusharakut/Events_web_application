using Application.Common;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Events.Update
{
    public class UploadEventImageUseCase : IUploadEventImageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _imageStoragePath = "wwwroot/images/events";

        public UploadEventImageUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int eventId, IFormFile imageFile, CancellationToken cancellationToken)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Invalid image file.");

            if (imageFile.Length > FileConstants.MaxFileSize)
                throw new ArgumentException("File size exceeds the maximum allowed size of 5 MB.");

            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!FileConstants.AllowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");

            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(eventId, cancellationToken);
            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found.");

            string newFileName = $"{eventId}{fileExtension}";
            string filePath = Path.Combine(_imageStoragePath, newFileName);

            if (!Directory.Exists(_imageStoragePath))
            {
                Directory.CreateDirectory(_imageStoragePath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream, cancellationToken);
            }

            await _unitOfWork.EventRepository.UpdateImagePathAsync(eventEntity, filePath, cancellationToken);
        }
    }

}

