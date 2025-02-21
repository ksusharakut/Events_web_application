using Domain.Interfaces;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IEventRepository EventRepository { get; }
        public IParticipantRepository ParticipantRepository { get; }
        public IParticipantEventRepository ParticipantEventRepository { get; }
        public IAuthRepository AuthRepository { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IEventRepository eventRepository,
            IParticipantRepository participantRepository,
            IParticipantEventRepository participantEventRepository,
            IAuthRepository authRepository)
        {
            _context = context;
            EventRepository = eventRepository;
            ParticipantRepository = participantRepository;
            ParticipantEventRepository = participantEventRepository;
            AuthRepository = authRepository;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
