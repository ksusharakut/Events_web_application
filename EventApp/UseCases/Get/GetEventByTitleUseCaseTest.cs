using Application.Common;
using Application.UseCases.DTOs;
using Application.UseCases.Events.Get;
using Domain.Interfaces;
using Domain.Exceptions;
using Moq;
using AutoMapper;

namespace Application.Tests
{
    public class GetEventByTitleUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ReturnsEventReturnDTO_WhenEventExists()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockImagePathService = new Mock<IImagePathService>();

            var eventEntity = TestDataGenerator.GenerateEvents(1).First(); 
            var eventReturnDTO = TestDataGenerator.GenerateEventReturnDTOs(1).First(); 

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetByTitleAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(eventEntity);

            mockMapper.Setup(mapper => mapper.Map<EventReturnDTO>(eventEntity))
                      .Returns(eventReturnDTO);

            mockImagePathService.Setup(service => service.GetImageUrl(It.IsAny<string>()))
                               .Returns((string url) => $"http://example.com/{url}");

            var useCase = new GetEventByTitleUseCase(mockUnitOfWork.Object, mockMapper.Object, mockImagePathService.Object);

            // Act
            var result = await useCase.ExecuteAsync("Test Event", CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventReturnDTO.Id, result.Id);
            Assert.Equal(eventReturnDTO.Title, result.Title);
            Assert.StartsWith("http://example.com/", result.ImageUrl);

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByTitleAsync("Test Event", It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(mapper => mapper.Map<EventReturnDTO>(eventEntity), Times.Once);
            mockImagePathService.Verify(service => service.GetImageUrl(eventEntity.ImageUrl), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockImagePathService = new Mock<IImagePathService>();

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetByTitleAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Domain.Entities.Event)null);

            var useCase = new GetEventByTitleUseCase(mockUnitOfWork.Object, mockMapper.Object, mockImagePathService.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync("Non-existent Event", CancellationToken.None));
            Assert.Equal("No event with Non-existent Event title.", exception.Message);

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetByTitleAsync("Non-existent Event", It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(mapper => mapper.Map<EventReturnDTO>(It.IsAny<Domain.Entities.Event>()), Times.Never);
            mockImagePathService.Verify(service => service.GetImageUrl(It.IsAny<string>()), Times.Never);
        }
    }
}