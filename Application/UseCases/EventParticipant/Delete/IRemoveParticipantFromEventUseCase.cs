namespace Application.UseCases.EventParticipant.Delete
{
    public interface IRemoveParticipantFromEventUseCase
    {
        Task ExecuteAsync(int eventId, CancellationToken cancellationToken);
    }
}
