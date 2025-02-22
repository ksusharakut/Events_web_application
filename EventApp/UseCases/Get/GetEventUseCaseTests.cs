using Application.UseCases.Events.Get;
using Application.UseCases.DTOs;
using Domain.Interfaces;
using Domain.Entities;
using AutoMapper;
using Moq;
using Domain.Exceptions;
using System.Threading.Tasks;
using System.Threading;
using Xunit;
using Application.Tests;
using Application.Common;

public class GetEventUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnEvent_WhenEventExists()
    {
        // Arrange
        var eventId = 1;
        var eventEntity = TestDataGenerator.GenerateEvent(eventId); 
        var eventDto = new EventReturnDTO
        {
            Id = eventEntity.Id,
            Title = eventEntity.Title,
            Description = eventEntity.Description,
            DateTime = eventEntity.DateTime,
            Location = eventEntity.Location,
            Category = eventEntity.Category,
            MaxParticipants = eventEntity.MaxParticipants,
            ImageUrl = $"http://example.com/{eventEntity.ImageUrl}"
        };

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(eventEntity);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(mapper => mapper.Map<EventReturnDTO>(eventEntity))
                  .Returns(eventDto);

        var mockImagePathService = new Mock<IImagePathService>();
        mockImagePathService.Setup(service => service.GetImageUrl(eventEntity.ImageUrl))
                            .Returns(eventDto.ImageUrl);

        var useCase = new GetEventUseCase(mockUnitOfWork.Object, mockMapper.Object, mockImagePathService.Object);

        // Act
        var result = await useCase.ExecuteAsync(eventId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(eventDto, result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = 1;

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(uow => uow.EventRepository.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync((Event)null);

        var mockMapper = new Mock<IMapper>();
        var mockImagePathService = new Mock<IImagePathService>();

        var useCase = new GetEventUseCase(mockUnitOfWork.Object, mockMapper.Object, mockImagePathService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(eventId, CancellationToken.None));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowArgumentException_WhenEventIdIsInvalid()
    {
        // Arrange
        var eventId = 0;

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockMapper = new Mock<IMapper>();
        var mockImagePathService = new Mock<IImagePathService>();

        var useCase = new GetEventUseCase(mockUnitOfWork.Object, mockMapper.Object, mockImagePathService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(eventId, CancellationToken.None));
    }
}