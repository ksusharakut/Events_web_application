using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Participant participant, CancellationToken cancellationToken)
        {
            await _context.Participants.AddAsync(participant, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Participant?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Participants.FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken)
        {
            var participant = await GetByEmailAsync(email, cancellationToken);

            if (participant == null)
                return false;

            return participant.PasswordHash == password;
        }
    }
}
