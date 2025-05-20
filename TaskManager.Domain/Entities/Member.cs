

using TaskManager.Domain.Enums;


namespace TaskManager.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TeamId { get; set; }
        public Team Team { get; set; } = null!;

        public Guid UserId { get; set; }

        public string Email { get; set; } = null!;

        public Guid? AssignedToMemberId { get; set; }
        public Member? AssignedTo { get; set; }

        public MemberRole Role { get; set; } = MemberRole.Member;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }
}
