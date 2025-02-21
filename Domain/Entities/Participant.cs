using Domain.Enums;

namespace Domain.Entities
{
    public class Participant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string PasswordHash { get; set; }

        public List<ParticipantEvent> Events { get; set; }
    }
}
