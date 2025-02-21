using Application.UseCases.DTOs;

namespace Application.UseCases.Events.Get
{
    public interface IGetEventByTitleUseCase
    {
        Task<EventReturnDTO> ExecuteAsync(string title, CancellationToken cancellationToken);
    }
}
