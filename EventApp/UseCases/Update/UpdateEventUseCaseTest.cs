using Application.Common;
using Application.UseCases.DTOs;
using Application.UseCases.Events.Update;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Application.Tests
{
    public class UpdateEventUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_UpdatesEvent_WhenValidationPassesAndEventExists()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EventDTO>>();
            var mockFileService = new Mock<IFileService>(); // Мок для IFileService

            int eventId = 1;
            var eventDto = TestDataGenerator.GenerateEventDTO();
            var existingEvent = TestDataGenerator.GenerateEvent(eventId);

            var validationResult = new ValidationResult();
            mockValidator.Setup(v => v.ValidateAsync(eventDto, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingEvent);

            mockMapper.Setup(m => m.Map(eventDto, existingEvent)).Verifiable();

            mockUnitOfWork.Setup(uow => uow.EventRepository.UpdateAsync(existingEvent, It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            var useCase = new UpdateEventUseCase(mockUnitOfWork.Object, mockMapper.Object, mockValidator.Object, mockFileService.Object); // Передача mockFileService

            // Act
            await useCase.ExecuteAsync(eventId, eventDto, CancellationToken.None);

            // Assert
            mockValidator.Verify(v => v.ValidateAsync(eventDto, It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map(eventDto, existingEvent), Times.Once);
            mockUnitOfWork.Verify(uow => uow.EventRepository.UpdateAsync(existingEvent, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsValidationException_WhenValidationFails()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EventDTO>>();
            var mockFileService = new Mock<IFileService>(); // Мок для IFileService

            int eventId = 1;
            var eventDto = TestDataGenerator.GenerateEventDTO();

            var validationErrors = new ValidationFailure[]
            {
                new ValidationFailure("Title", "Title is required")
            };
            var validationResult = new ValidationResult(validationErrors);
            mockValidator.Setup(v => v.ValidateAsync(eventDto, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);

            var useCase = new UpdateEventUseCase(mockUnitOfWork.Object, mockMapper.Object, mockValidator.Object, mockFileService.Object); // Передача mockFileService

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => useCase.ExecuteAsync(eventId, eventDto, CancellationToken.None));
            Assert.Equal("Title is required", exception.Errors.First().ErrorMessage);

            mockValidator.Verify(v => v.ValidateAsync(eventDto, It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            mockMapper.Verify(m => m.Map(It.IsAny<EventDTO>(), It.IsAny<Event>()), Times.Never);
            mockUnitOfWork.Verify(uow => uow.EventRepository.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockValidator = new Mock<IValidator<EventDTO>>();
            var mockFileService = new Mock<IFileService>(); // Мок для IFileService

            int eventId = 1;
            var eventDto = TestDataGenerator.GenerateEventDTO();

            var validationResult = new ValidationResult();
            mockValidator.Setup(v => v.ValidateAsync(eventDto, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Event)null);

            var useCase = new UpdateEventUseCase(mockUnitOfWork.Object, mockMapper.Object, mockValidator.Object, mockFileService.Object); // Передача mockFileService

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(eventId, eventDto, CancellationToken.None));
            Assert.Equal($"Event with ID {eventId} not found.", exception.Message);

            mockValidator.Verify(v => v.ValidateAsync(eventDto, It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map(It.IsAny<EventDTO>(), It.IsAny<Event>()), Times.Never);
            mockUnitOfWork.Verify(uow => uow.EventRepository.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
