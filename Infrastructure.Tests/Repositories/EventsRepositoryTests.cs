using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories
{
    public class EventsRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventRepository _repository;

        public EventsRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new EventRepository(_context);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var eventEntity = new Event { Id = 1, Title = "Test Event", DateTime = DateTime.Now, Location = "Test Location", Category = "Test Category" };
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventEntity.Title, result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenEventDoesNotExist()
        {
            // Arrange
            // Ничего не добавляем в базу данных

            // Act
            var result = await _repository.GetByIdAsync(1, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEventToDatabase()
        {
            // Arrange
            var eventEntity = new Event { Id = 2, Title = "New Event", DateTime = DateTime.Now, Location = "New Location", Category = "New Category" };

            // Act
            await _repository.AddAsync(eventEntity, CancellationToken.None);

            // Assert
            var savedEvent = await _context.Events.FindAsync(2);
            Assert.NotNull(savedEvent);
            Assert.Equal(eventEntity.Title, savedEvent.Title);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenEventIsNull()
        {
            // Arrange
            Event? eventEntity = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAsync(eventEntity, CancellationToken.None));
        }
    }
}
