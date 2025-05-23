

using MediatR;

namespace TaskManager.Application.Features.Teams.Commands.UpdateTeam;

public class UpdateTeamCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty!;
    public string Description { get; set; } = string.Empty!;
}
