using Application.UseCases.Events.Delete;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Moq;

namespace Application.Tests
{
    public class DeleteEventUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeletesEvent_WhenEventExists()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            int eventId = 1;
            var eventEntity = TestDataGenerator.GenerateEvent(eventId); 

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(eventEntity);

            mockUnitOfWork.Setup(uow => uow.EventRepository.DeleteAsync(eventEntity, It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            var useCase = new DeleteEventUseCase(mockUnitOfWork.Object);

            // Act
            await useCase.ExecuteAsync(eventId, CancellationToken.None);

            // Assert
            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.EventRepository.DeleteAsync(eventEntity, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            int eventId = 1;

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Event)null);

            var useCase = new DeleteEventUseCase(mockUnitOfWork.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(eventId, CancellationToken.None));
            Assert.Equal($"No event with id {eventId}.", exception.Message);

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.EventRepository.DeleteAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}