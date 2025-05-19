

namespace TaskManager.Domain.Entities
{
    public class ActivityLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TeamId { get; set; }
        public Team Team { get; set; } = null!;

        public string Action { get; set; } = null!; // e.g., "Task Created", "User Invited"

        public string PerformedBy { get; set; } = null!; // Name or Email of actor

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string? Details { get; set; } // Optional additional info
    }
}
