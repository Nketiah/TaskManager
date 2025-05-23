

using MediatR;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Features.Teams.Commands.DeleteTeam;

public class DeleteTeamCommandHanler : IRequestHandler<DeleteTeamCommand, Unit>
{
    private readonly ITeamRepository _teamRepository;
    public DeleteTeamCommandHanler(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }
    public async Task<Unit> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdAsync(request.Id);
        if (team == null)
        {
            throw new NotFountException(nameof(Team), request.Id);
        }
        await _teamRepository.DeleteAsync(team);
        return Unit.Value;
    }
}

