using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IParticipantRepository
    {
        Task<Participant?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<(IEnumerable<Participant> Participants, int TotalCount)> GetByEventIdAsync(CancellationToken cancellationToken, int eventId, int pageNumber = 1, int pageSize = 10);
        Task AddAsync(Participant participant, CancellationToken cancellationToken);
        Task DeleteAsync(Participant participant, CancellationToken cancellationToken);
        Task<Participant?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    }
}
