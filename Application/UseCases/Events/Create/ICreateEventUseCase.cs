using Application.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Create
{
    public interface ICreateEventUseCase
    {
        Task ExecuteAsync(EventDTO eventDto, CancellationToken cancellationToken);
    }
}