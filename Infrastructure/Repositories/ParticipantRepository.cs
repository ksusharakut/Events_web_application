using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly ApplicationDbContext _context;

        public ParticipantRepository(ApplicationDbContext context) 
        {
            _context = context; 
        }

        public async Task AddAsync(Participant participant, CancellationToken cancellationToken)
        {
            await _context.Participants.AddAsync(participant, cancellationToken);
        }

        public async Task DeleteAsync(Participant participant, CancellationToken cancellationToken)
        {
            _context.Participants.Remove(participant);
        }

        public async Task<Participant?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Participants.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<(IEnumerable<Participant> Participants, int TotalCount)> GetByEventIdAsync(CancellationToken cancellationToken, int eventId, int pageNumber = 1, int pageSize = 10)
        {
            var totalCount = await _context.ParticipantEvents
                .Where(pe => pe.EventId == eventId)
                .CountAsync(cancellationToken);

            var participants = await _context.ParticipantEvents
                .Where(pe => pe.EventId == eventId)
                .Select(pe => pe.Participant)
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize) 
                .ToListAsync(cancellationToken);

            return (participants, totalCount);
        }

        public async Task<Participant?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Participants.FindAsync(id, cancellationToken);
        }
    }
}
