

using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Member;

public class AddMemberRequestDto
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public MemberRole Role { get; set; } = MemberRole.Member;
}
