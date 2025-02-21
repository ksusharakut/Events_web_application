using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Events.Update
{
    public interface IUploadEventImageUseCase
    {
        Task ExecuteAsync(int eventId, IFormFile imageFile, CancellationToken cancellationToken);
    }
}
