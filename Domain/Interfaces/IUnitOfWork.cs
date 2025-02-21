﻿using Domain.Interfaces.RepositoryInterfaces;
namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository EventRepository { get; }
        IParticipantRepository ParticipantRepository { get; }
        IParticipantEventRepository ParticipantEventRepository { get; }

        IAuthRepository AuthRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken); 
    }
}
