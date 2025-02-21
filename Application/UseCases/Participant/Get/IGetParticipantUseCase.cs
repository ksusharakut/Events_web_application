using Application.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Participant.Get
{
    public interface IGetParticipantUseCase
    {
        Task<ParticipantReturnDTO> ExecuteAsync(int participantId, CancellationToken cancellationToken);
    }
}
