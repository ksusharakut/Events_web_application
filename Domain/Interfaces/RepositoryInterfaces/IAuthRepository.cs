using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IAuthRepository
    {
        Task<Participant?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddAsync(Participant participant, CancellationToken cancellationToken);
        Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken);
    }
}
