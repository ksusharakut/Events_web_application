using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IParticipantEventRepository
    {
        Task RegisterAsync(ParticipantEvent participantEvent, CancellationToken cancellationToken);
        Task RemoveAsync(ParticipantEvent participantEvent, CancellationToken cancellationToken);
        Task<ParticipantEvent?> GetByEventAndParticipantAsync(int eventId, int participantId, CancellationToken cancellationToken);
        Task<List<Participant>> GetParticipantsByEventIdAsync(Event eventEntity, CancellationToken cancellationToken);
        Task<int> GetParticipantsCountByEventAsync(int eventId, CancellationToken cancellationToken);
    }
}
