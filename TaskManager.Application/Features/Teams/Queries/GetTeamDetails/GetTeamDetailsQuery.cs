

using MediatR;

namespace TaskManager.Application.Features.Teams.Queries.GetTeamDetails;

public class GetTeamDetailsQuery : IRequest<TeamDetailsDto>
{
    public Guid Id { get; set; }
}

