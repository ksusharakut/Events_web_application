using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EventParticipant
{
    public interface IRemoveParticipantFromEventUseCase
    {
        Task ExecuteAsync(int eventId, CancellationToken cancellationToken);
    }
}
