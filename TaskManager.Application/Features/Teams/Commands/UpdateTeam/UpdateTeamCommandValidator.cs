

using FluentValidation;
using TaskManager.Application.Interfaces;

namespace TaskManager.Application.Features.Teams.Commands.UpdateTeam;

public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
{
    private readonly ITeamRepository _teamRepository;
    public UpdateTeamCommandValidator(ITeamRepository teamRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 50 characters.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 200 characters.");

        RuleFor(p => p)
            .MustAsync(TeamNameUnique)
            .WithMessage("Team name must be unique.");

        _teamRepository = teamRepository;
    }

    private async Task<bool> TeamNameUnique(UpdateTeamCommand command, CancellationToken token)
    {
        return await _teamRepository.IsTeamNameUniqueAsync(command.Name);
    }
}

