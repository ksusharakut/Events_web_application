using Application.Common;
using Application.UseCases.DTOs;
using Application.UseCases.Events.Get;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Moq;
using Domain.Interfaces;

namespace Application.Tests.UseCases.Get
{
    public class GetEventUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IImagePathService> _mockImagePathService;
        private readonly GetEventUseCase _getEventUseCase;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public GetEventUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockImagePathService = new Mock<IImagePathService>();
            _getEventUseCase = new GetEventUseCase(_mockUnitOfWork.Object, _mockMapper.Object, _mockImagePathService.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnEventDto_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var testEvent = TestDataGenerator.CreateTestEvent();
            var eventDto = new EventReturnDTO
            {
                Id = eventId,
                Title = "Test Event",
                DateTime = DateTime.Now,
                Location = "Test Location",
                Category = "Test Category",
                ImageUrl = "images/test.jpg"
            };

            _mockUnitOfWork
                .Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(testEvent);

            _mockMapper
                .Setup(mapper => mapper.Map<EventReturnDTO>(testEvent))
                .Returns(eventDto);

            _mockImagePathService
                .Setup(service => service.GetImageUrl(eventDto.ImageUrl))
                .Returns("https://example.com/images/test.jpg");

            // Act
            var result = await _getEventUseCase.ExecuteAsync(eventId, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventDto.Id, result.Id);
            Assert.Equal(eventDto.Title, result.Title);
            Assert.Equal("https://example.com/images/test.jpg", result.ImageUrl);

            _mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<EventReturnDTO>(testEvent), Times.Once);
            _mockImagePathService.Verify(service => service.GetImageUrl(eventDto.ImageUrl), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;

            _mockUnitOfWork
                .Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _getEventUseCase.ExecuteAsync(eventId, _cancellationToken)
            );

            Assert.Equal("Event not found.", exception.Message);
            _mockUnitOfWork.Verify(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<EventReturnDTO>(It.IsAny<Event>()), Times.Never);
            _mockImagePathService.Verify(service => service.GetImageUrl(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldMapEventToDtoCorrectly()
        {
            // Arrange
            var eventId = 1;
            var testEvent = TestDataGenerator.CreateTestEvent();
            var eventDto = new EventReturnDTO
            {
                Id = eventId,
                Title = "Test Event",
                DateTime = DateTime.Now,
                Location = "Test Location",
                Category = "Test Category",
                ImageUrl = "images/test.jpg"
            };

            _mockUnitOfWork
                .Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(testEvent);

            _mockMapper
                .Setup(mapper => mapper.Map<EventReturnDTO>(testEvent))
                .Returns(eventDto);

            _mockImagePathService
                .Setup(service => service.GetImageUrl(eventDto.ImageUrl))
                .Returns("https://example.com/images/test.jpg");

            // Act
            var result = await _getEventUseCase.ExecuteAsync(eventId, _cancellationToken);

            // Assert
            _mockMapper.Verify(mapper => mapper.Map<EventReturnDTO>(testEvent), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldUpdateImageUrlUsingImagePathService()
        {
            // Arrange
            var eventId = 1;
            var testEvent = TestDataGenerator.CreateTestEvent();
            var eventDto = new EventReturnDTO
            {
                Id = eventId,
                Title = "Test Event",
                DateTime = DateTime.Now,
                Location = "Test Location",
                Category = "Test Category",
                ImageUrl = "images/test.jpg"
            };

            _mockUnitOfWork
                .Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(testEvent);

            _mockMapper
                .Setup(mapper => mapper.Map<EventReturnDTO>(testEvent))
                .Returns(eventDto);

            _mockImagePathService
                .Setup(service => service.GetImageUrl(eventDto.ImageUrl))
                .Returns("https://example.com/images/test.jpg");

            // Act
            var result = await _getEventUseCase.ExecuteAsync(eventId, _cancellationToken);

            // Assert
            Assert.Equal("https://example.com/images/test.jpg", result.ImageUrl);
            _mockImagePathService.Verify(service => service.GetImageUrl(eventDto.ImageUrl), Times.Once);
        }
    }
}
