using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.DTOs
{
    public class EventReturnDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public int MaxParticipants { get; set; }
        public string? ImageUrl { get; set; }
    }
}
