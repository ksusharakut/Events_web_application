using Domain.Interfaces;

namespace Application.UseCases.Events.Delete
{
    public class DeleteEventUseCase : IDeleteEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEventUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int eventId, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(eventId, cancellationToken);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"No event with id {eventId}.");
            }

            await _unitOfWork.EventRepository.DeleteAsync(eventEntity, cancellationToken);
        }
    }
}
