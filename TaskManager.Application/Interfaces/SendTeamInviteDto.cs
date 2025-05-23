

namespace TaskManager.Application.Interfaces
{
    public class SendTeamInviteDto
    {
        public Guid TeamId { get; set; }
        public string Email { get; set; } = null!;
    }
}
