using Application.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Get
{
    public interface IGetEventsByCriteriaUseCase
    {
        Task<List<EventReturnDTO>> ExecuteAsync(DateTime? date, string? location, string? category, CancellationToken cancellationToken);
    }
}
