

namespace TaskManager.Domain.Entities;

public class Team
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid OwnerId { get; set; } // ID of the user who created the team

    // Navigation Properties
    public ICollection<Member> Members { get; set; } = new List<Member>();

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
}
