using AutoMapper;
using Application.Common;
using Application.UseCases.DTOs;
using Application.UseCases.Events.Get;
using Domain.Interfaces;
using Moq;

namespace Application.Tests
{
    public class GetAllEventsUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ReturnsCorrectData()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockImagePathService = new Mock<IImagePathService>();

            var events = TestDataGenerator.GenerateEvents(2); 
            var eventDtos = TestDataGenerator.GenerateEventReturnDTOs(2); 

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<int>()))
                          .ReturnsAsync(events);

            mockMapper.Setup(mapper => mapper.Map<IEnumerable<EventReturnDTO>>(events))
                      .Returns(eventDtos);

            mockImagePathService.Setup(service => service.GetImageUrl(It.IsAny<string>()))
                                 .Returns((string url) => $"http://example.com/{url}");

            var useCase = new GetAllEventsUseCase(mockUnitOfWork.Object, mockMapper.Object, mockImagePathService.Object);

            // Act
            var result = await useCase.ExecuteAsync(CancellationToken.None, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            foreach (var eventDto in result)
            {
                Assert.StartsWith("http://example.com/", eventDto.ImageUrl);
            }

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetAllAsync(It.IsAny<CancellationToken>(), 1, 10), Times.Once);
            mockMapper.Verify(mapper => mapper.Map<IEnumerable<EventReturnDTO>>(events), Times.Once);
            mockImagePathService.Verify(service => service.GetImageUrl(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsArgumentException_WhenPageNumberOrPageSizeIsInvalid()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockImagePathService = new Mock<IImagePathService>();

            var useCase = new GetAllEventsUseCase(mockUnitOfWork.Object, mockMapper.Object, mockImagePathService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(CancellationToken.None, 0, 10));
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(CancellationToken.None, 1, 0));
        }
    }
}