

namespace TaskManager.Application.DTOs.Team;

public class UpdateTeamRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
