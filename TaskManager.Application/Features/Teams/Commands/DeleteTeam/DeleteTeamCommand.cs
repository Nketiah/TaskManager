

using MediatR;

namespace TaskManager.Application.Features.Teams.Commands.DeleteTeam;

public class DeleteTeamCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

