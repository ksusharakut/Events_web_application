using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ParticipantEventRepository : IParticipantEventRepository
    {
        private readonly ApplicationDbContext _context;

        public ParticipantEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ParticipantEvent?> GetByEventAndParticipantAsync(int eventId, int participantId, CancellationToken cancellationToken)
        {
            return await _context.ParticipantEvents
                .Where(pe => pe.EventId == eventId && pe.ParticipantId == participantId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Participant>> GetParticipantsByEventIdAsync(Event eventEntity, CancellationToken cancellationToken)
        {
            return await _context.ParticipantEvents
                .Where(pe => pe.EventId == eventEntity.Id)
                .Select(pe => pe.Participant)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetParticipantsCountByEventAsync(int eventId, CancellationToken cancellationToken)
        {
            return await _context.ParticipantEvents
                .CountAsync(pe => pe.EventId == eventId, cancellationToken);
        }

        public async Task RegisterAsync(ParticipantEvent participantEvent, CancellationToken cancellationToken)
        {
            await _context.ParticipantEvents.AddAsync(participantEvent, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(ParticipantEvent participantEvent, CancellationToken cancellationToken)
        {
            _context.ParticipantEvents.Remove(participantEvent);  
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}
