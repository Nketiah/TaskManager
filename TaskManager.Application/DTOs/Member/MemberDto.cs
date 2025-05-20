

using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Member;

public class MemberDto
{
    public Guid Id { get; set; }

    public Guid TeamId { get; set; }

    public Guid UserId { get; set; }

    public string Email { get; set; } = null!;

    public MemberRole Role { get; set; }

    public DateTime JoinedAt { get; set; }
}
