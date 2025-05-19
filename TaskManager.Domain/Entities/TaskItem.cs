

using TaskManager.Domain.Enums;
namespace TaskManager.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        public TStatus Status { get; set; } = TStatus.Pending;

        public Guid? AssignedToMemberId { get; set; }
        public Member? AssignedTo { get; set; }

        public Guid TeamId { get; set; }
        public Team Team { get; set; } = null!;
    }
}
