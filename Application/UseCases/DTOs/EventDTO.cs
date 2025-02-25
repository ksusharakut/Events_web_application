using Microsoft.AspNetCore.Http;

namespace Application.UseCases.DTOs
{
    public class EventDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public int MaxParticipants { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
