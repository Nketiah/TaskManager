
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class TeamInvitation
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TeamId { get; set; }
        public Team Team { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Token { get; set; } = Guid.NewGuid().ToString();

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RespondedAt { get; set; }
    }
}
