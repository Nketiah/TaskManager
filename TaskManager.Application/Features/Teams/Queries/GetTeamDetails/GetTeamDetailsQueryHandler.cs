

using AutoMapper;
using MediatR;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Features.Teams.Queries.GetTeamDetails;

public class GetTeamDetailsQueryHandler : IRequestHandler<GetTeamDetailsQuery, TeamDetailsDto>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;
    public GetTeamDetailsQueryHandler(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }
    public async Task<TeamDetailsDto> Handle(GetTeamDetailsQuery request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdAsync(request.Id);
        if (team == null)
        {
            throw new NotFountException(nameof(Team), request.Id);
        }

        return _mapper.Map<TeamDetailsDto>(team);
    }
}

