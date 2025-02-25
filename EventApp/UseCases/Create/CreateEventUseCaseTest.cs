using Application.Common;
using Application.UseCases.DTOs;
using Application.UseCases.Events.Create;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Application.Tests
{
    public class CreateEventUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_CreatesEvent_WhenValidationPasses()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EventDTO>>();
            var mockFileService = new Mock<IFileService>();  // Мок для IFileService

            var eventDto = TestDataGenerator.GenerateEventDTO();
            var eventEntity = new Event();

            var validationResult = new ValidationResult();
            mockValidator.Setup(v => v.ValidateAsync(eventDto, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);

            mockMapper.Setup(m => m.Map<Event>(eventDto))
                      .Returns(eventEntity);

            mockUnitOfWork.Setup(uow => uow.EventRepository.AddAsync(eventEntity, It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            var useCase = new CreateEventUseCase(mockUnitOfWork.Object, mockMapper.Object, mockValidator.Object, mockFileService.Object);

            // Act
            await useCase.ExecuteAsync(eventDto, CancellationToken.None);

            // Assert
            mockValidator.Verify(v => v.ValidateAsync(eventDto, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<Event>(eventDto), Times.Once);
            mockUnitOfWork.Verify(uow => uow.EventRepository.AddAsync(eventEntity, It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsValidationException_WhenValidationFails()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EventDTO>>();
            var mockFileService = new Mock<IFileService>();  // Мок для IFileService

            var eventDto = TestDataGenerator.GenerateEventDTO();

            var validationErrors = new ValidationFailure[]
            {
                new ValidationFailure("Title", "Title is required")
            };
            var validationResult = new ValidationResult(validationErrors);
            mockValidator.Setup(v => v.ValidateAsync(eventDto, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);

            var useCase = new CreateEventUseCase(mockUnitOfWork.Object, mockMapper.Object, mockValidator.Object, mockFileService.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => useCase.ExecuteAsync(eventDto, CancellationToken.None));
            Assert.Equal("Title is required", exception.Errors.First().ErrorMessage);

            mockValidator.Verify(v => v.ValidateAsync(eventDto, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<Event>(It.IsAny<EventDTO>()), Times.Never);
            mockUnitOfWork.Verify(uow => uow.EventRepository.AddAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Never);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
