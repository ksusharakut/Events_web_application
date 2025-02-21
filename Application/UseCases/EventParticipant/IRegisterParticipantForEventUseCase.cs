using Application.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EventParticipant
{
    public interface IRegisterParticipantForEventUseCase
    {
        Task ExecuteAsync(RegisterParticipantDTO request, CancellationToken cancellationToken);
    }
}
