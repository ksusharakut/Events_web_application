﻿using Application.UseCases.DTOs;

namespace Application.UseCases.Events.Get
{
    public interface IGetEventsByCriteriaUseCase
    {
        Task<List<EventReturnDTO>> ExecuteAsync(DateTime? date, string? location, string? category, CancellationToken cancellationToken);
    }
}
