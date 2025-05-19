using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskManager.Application.DTOs.Team;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;


namespace TaskManager.Application.Teams;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly ILogger<TeamService> _logger;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepository, ILogger<TeamService> logger, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TeamDto> CreateTeamAsync(CreateTeamRequestDto request)
    {
        _logger.LogInformation("Creating a new team with name: {Name}", request.Name);
        var team = _mapper.Map<Team>(request);
        team.Id = Guid.NewGuid();
        team.CreatedAt = DateTime.UtcNow;
        await _teamRepository.AddAsync(team);

        var teamDto = _mapper.Map<TeamDto>(team);
        return teamDto;
    }

    public async Task<bool> DeleteTeamAsync(Guid teamId)
    {
        _logger.LogInformation("Deleting team with ID: {TeamId}", teamId);
        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
        {
            _logger.LogWarning("Team with ID {TeamId} not found.", teamId);
            return false;
        }
        await _teamRepository.DeleteAsync(team);
        return true;
    }

    public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync()
    {
        _logger.LogInformation("Getting All Teams");
        var teams = await _teamRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }

    public async Task<TeamDto> GetTeamByIdAsync(Guid teamId)
    {
        _logger.LogInformation($"Getting team ${teamId}");
        var team = await _teamRepository.GetByIdAsync(teamId);

        if (team == null)
            throw new KeyNotFoundException($"Team with ID '{teamId}' was not found.");

        return _mapper.Map<TeamDto>(team);
    }


    public async Task<bool> UpdateTeamAsync(Guid teamId, UpdateTeamRequestDto request)
    {
        _logger.LogInformation($"Updating team with id ${teamId}");
        var existingTeam = await _teamRepository.GetByIdAsync(teamId);

        if (existingTeam == null)
            return false;

        _mapper.Map(request, existingTeam);
        await _teamRepository.UpdateAsync(existingTeam);

        return true;
    }

}
