

using MediatR;

namespace TaskManager.Application.Features.Teams.Commands.CreateTeam;

public class CreateTeamCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty!;
    public string Description { get; set; } = string.Empty!;
}

