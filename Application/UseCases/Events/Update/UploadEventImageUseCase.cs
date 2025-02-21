using Domain.Interfaces.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Update
{
    public class UploadEventImageUseCase : IUploadEventImageUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly string _imageStoragePath = "wwwroot/images/events";
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public UploadEventImageUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task ExecuteAsync(int eventId, IFormFile imageFile, CancellationToken cancellationToken)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Invalid image file.");

            if (imageFile.Length > MaxFileSize)
                throw new ArgumentException("File size exceeds the maximum allowed size of 5 MB.");

            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!AllowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");

            var eventEntity = await _eventRepository.GetByIdAsync(eventId, cancellationToken);
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

            await _eventRepository.UpdateImagePathAsync(eventEntity, filePath, cancellationToken);
        }
    }

}

