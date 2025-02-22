using Application.UseCases.DTOs;
using Application.UseCases.Events.Get;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.Tests
{
    public class GetEventsByCriteriaUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ReturnsFilteredEvents_WhenCriteriaMatch()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            var events = TestDataGenerator.GenerateEvents(2); 
            var eventDtos = TestDataGenerator.GenerateEventReturnDTOs(2); 

            DateTime date = DateTime.UtcNow;
            string location = "Test Location";
            string category = "Test Category";

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetEventsByFiltersAsync(date, location, category, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(events);

            mockMapper.Setup(mapper => mapper.Map<List<EventReturnDTO>>(events))
                      .Returns(eventDtos);

            var useCase = new GetEventsByCriteriaUseCase(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await useCase.ExecuteAsync(date, location, category, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetEventsByFiltersAsync(date, location, category, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(mapper => mapper.Map<List<EventReturnDTO>>(events), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsEmptyList_WhenNoEventsMatchCriteria()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            DateTime date = DateTime.UtcNow;
            string location = "Non-existent Location";
            string category = "Non-existent Category";

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetEventsByFiltersAsync(date, location, category, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new List<Event>());

            mockMapper.Setup(mapper => mapper.Map<List<EventReturnDTO>>(It.IsAny<List<Event>>()))
                      .Returns(new List<EventReturnDTO>());

            var useCase = new GetEventsByCriteriaUseCase(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await useCase.ExecuteAsync(date, location, category, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetEventsByFiltersAsync(date, location, category, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(mapper => mapper.Map<List<EventReturnDTO>>(It.IsAny<List<Event>>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsAllEvents_WhenNoCriteriaProvided()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            var events = TestDataGenerator.GenerateEvents(3); 
            var eventDtos = TestDataGenerator.GenerateEventReturnDTOs(3); 

            mockUnitOfWork.Setup(uow => uow.EventRepository.GetEventsByFiltersAsync(null, null, null, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(events);

            mockMapper.Setup(mapper => mapper.Map<List<EventReturnDTO>>(events))
                      .Returns(eventDtos);

            var useCase = new GetEventsByCriteriaUseCase(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await useCase.ExecuteAsync(null, null, null, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            mockUnitOfWork.Verify(uow => uow.EventRepository.GetEventsByFiltersAsync(null, null, null, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(mapper => mapper.Map<List<EventReturnDTO>>(events), Times.Once);
        }
    }
}