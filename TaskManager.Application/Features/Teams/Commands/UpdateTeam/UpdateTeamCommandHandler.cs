

using AutoMapper;
using MediatR;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Features.Teams.Commands.UpdateTeam;

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Unit>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;
    public UpdateTeamCommandHandler(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateTeamCommandValidator(_teamRepository);
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.Errors.Any())
        {
            throw new BadRequestException("Invalid Team", validationResult);
        }

        var team = await _teamRepository.GetByIdAsync(request.Id);
        if (team == null)
        {
            throw new NotFountException(nameof(Team), request.Id);
        }

        var teamToUpdate = _mapper.Map<Team>(request);
        await _teamRepository.UpdateAsync(teamToUpdate);
        return Unit.Value;
    }
}
