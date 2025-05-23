using TaskManager.Application.DTOs.Member;

namespace TaskManager.Application.Features.Teams.Queries.GetAllTeams;

public class TeamDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int MemberCount { get; set; }

    public List<MemberDto> Members { get; set; } = new();
}
