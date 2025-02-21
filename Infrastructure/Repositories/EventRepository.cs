using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Event eventEntity, CancellationToken cancellationToken)
        {
            await _context.Events.AddAsync(eventEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Event eventEntity, CancellationToken cancellationToken)
        {
            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateImagePathAsync(Event eventEntity, string imagePath, CancellationToken cancellationToken)
        {
            eventEntity.ImageUrl = imagePath;
            _context.Events.Update(eventEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken, int pageNumber, int pageSize)
        {
            return await _context.Events
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync(cancellationToken);
        }

        public async Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Events.FindAsync(id, cancellationToken);
        }

        public async Task<Event?> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            return await _context.Events.FirstOrDefaultAsync(x => x.Title == title, cancellationToken);
        }

        public async Task UpdateAsync(Event eventEntity, CancellationToken cancellationToken)
        {
            _context.Events.Update(eventEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Event>> GetEventsByFiltersAsync(DateTime? date, string location, string category, CancellationToken cancellationToken)
        {
            var query = _context.Events.AsQueryable();

            query = date != null ? query.Where(e => e.DateTime.Date == date.Value.Date) : query;
            query = location != null ? query.Where(e => e.Location == location) : query;
            query = category != null ? query.Where(e => e.Category == category) : query;

            return await query.ToListAsync(cancellationToken);
        }
    }
}
