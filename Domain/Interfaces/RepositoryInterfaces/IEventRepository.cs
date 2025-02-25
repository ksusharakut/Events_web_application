using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IEventRepository
    {
        Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Event?> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken, int pageNumber, int pageSize);
        Task AddAsync(Event eventEntity, CancellationToken cancellationToken);
        Task UpdateAsync(Event eventEntity, CancellationToken cancellationToken);
        Task DeleteAsync(Event eventEntity, CancellationToken cancellationToken);
        Task<List<Event>> GetEventsByFiltersAsync(DateTime? date, string location, string category, CancellationToken cancellationToken);
    }
}
