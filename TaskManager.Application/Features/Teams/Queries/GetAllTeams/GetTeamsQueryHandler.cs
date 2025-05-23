

using AutoMapper;
using MediatR;
using TaskManager.Application.Interfaces;

namespace TaskManager.Application.Features.Teams.Queries.GetAllTeams;

public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, IEnumerable<TeamDto>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;
    public GetTeamsQueryHandler(ITeamRepository teamService, IMapper mapper)
    {
        _teamRepository = teamService;
        _mapper = mapper;
    }
    public async Task<IEnumerable<TeamDto>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
    {
        var teams = await _teamRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }
}

