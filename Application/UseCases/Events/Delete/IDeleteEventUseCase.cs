namespace Application.UseCases.Events.Delete
{
    public interface IDeleteEventUseCase
    {
        Task ExecuteAsync(int eventId, CancellationToken cancellationToken);
    }
}
