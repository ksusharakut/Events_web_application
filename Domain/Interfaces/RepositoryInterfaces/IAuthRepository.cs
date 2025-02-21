using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IAuthRepository
    {
        Task<Participant?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddAsync(Participant participant, CancellationToken cancellationToken);
        Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken);
    }
}
