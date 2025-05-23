

using MediatR;

namespace TaskManager.Application.Features.Teams.Queries.GetAllTeams;

public class GetTeamsQuery : IRequest<IEnumerable<TeamDto>>
{
}

