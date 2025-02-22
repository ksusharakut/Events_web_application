using Application.Common;
using Application.UseCases.Events.Update;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Application.Tests
{
    public class UploadEventImageUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_UploadsImage_WhenFileIsValidAndEventExists()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFormFile = new Mock<IFormFile>();

            int eventId = 1;
            var eventEntity = TestDataGenerator.GenerateEvent(eventId); 
            string fileExtension = ".jpg";
            string fileName = $"{eventId}{fileExtension}";
            string filePath = Path.Combine("wwwroot/images/events", fileName);

            mockFormFile.Setup(f => f.Length).Returns(1024); 
            mockFormFile.Setup(f => f.FileName).Returns(fileName);
            mockFormFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                        .Returns(Task.CompletedTask);

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(eventEntity);

            mockUnitOfWork.Setup(uow => uow.EventRepository.UpdateImagePathAsync(eventEntity, filePath, It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            var useCase = new UploadEventImageUseCase(mockUnitOfWork.Object);

            // Act
            await useCase.ExecuteAsync(eventId, mockFormFile.Object, CancellationToken.None);

            // Assert
            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.EventRepository.UpdateImagePathAsync(eventEntity, filePath, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsArgumentException_WhenFileIsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            int eventId = 1;
            var useCase = new UploadEventImageUseCase(mockUnitOfWork.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(eventId, null, CancellationToken.None));
            Assert.Equal("Invalid image file.", exception.Message);

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            mockUnitOfWork.Verify(uow => uow.EventRepository.UpdateImagePathAsync(It.IsAny<Event>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsArgumentException_WhenFileSizeExceedsLimit()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFormFile = new Mock<IFormFile>();

            int eventId = 1;
            mockFormFile.Setup(f => f.Length).Returns(FileConstants.MaxFileSize + 1); 

            var useCase = new UploadEventImageUseCase(mockUnitOfWork.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(eventId, mockFormFile.Object, CancellationToken.None));
            Assert.Equal("File size exceeds the maximum allowed size of 5 MB.", exception.Message);

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            mockUnitOfWork.Verify(uow => uow.EventRepository.UpdateImagePathAsync(It.IsAny<Event>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsArgumentException_WhenFileTypeIsInvalid()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFormFile = new Mock<IFormFile>();

            int eventId = 1;
            mockFormFile.Setup(f => f.Length).Returns(1024); 
            mockFormFile.Setup(f => f.FileName).Returns("test.txt"); 

            var useCase = new UploadEventImageUseCase(mockUnitOfWork.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(eventId, mockFormFile.Object, CancellationToken.None));
            Assert.Equal("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.", exception.Message);

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            mockUnitOfWork.Verify(uow => uow.EventRepository.UpdateImagePathAsync(It.IsAny<Event>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFormFile = new Mock<IFormFile>();

            int eventId = 1;
            mockFormFile.Setup(f => f.Length).Returns(1024); // 1 KB
            mockFormFile.Setup(f => f.FileName).Returns("test.jpg");

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Event)null);

            var useCase = new UploadEventImageUseCase(mockUnitOfWork.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(eventId, mockFormFile.Object, CancellationToken.None));
            Assert.Equal("Event not found.", exception.Message);

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.EventRepository.UpdateImagePathAsync(It.IsAny<Event>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}