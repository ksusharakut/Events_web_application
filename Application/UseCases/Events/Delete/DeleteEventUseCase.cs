using Domain.Interfaces;
using Domain.Exceptions;
using Application.Common;

namespace Application.UseCases.Events.Delete
{
    public class DeleteEventUseCase : IDeleteEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteEventUseCase(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task ExecuteAsync(int eventId, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(eventId, cancellationToken);
            if (eventEntity == null)
            {
                throw new NotFoundException($"No event with id {eventId}.");
            }

            if (!string.IsNullOrEmpty(eventEntity.ImageUrl))
            {
                await _fileService.DeleteFileAsync(eventEntity.ImageUrl, cancellationToken);
            }

            await _unitOfWork.EventRepository.DeleteAsync(eventEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
