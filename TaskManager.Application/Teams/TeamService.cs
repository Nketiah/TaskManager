using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskManager.Application.DTOs.Member;
using TaskManager.Application.DTOs.Team;
using TaskManager.Application.Features.Teams.Queries.GetAllTeams;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;


namespace TaskManager.Application.Teams;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly ILogger<TeamService> _logger;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public TeamService(ITeamRepository teamRepository, ILogger<TeamService> logger, IMapper mapper, IEmailService emailService)
    {
        _teamRepository = teamRepository;
        _logger = logger;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<TeamDto> CreateTeamAsync(Team teamRequest)
    {
        _logger.LogInformation("Creating a new team with name: {Name}", teamRequest.Name);
        var team = _mapper.Map<Team>(teamRequest);
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


    public async Task<Team?> UpdateTeamAsync(Guid teamId, UpdateTeamRequestDto request)
    {
        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
        {
            return null;
        }

        team.Name = request.Name;
        team.Description = request.Description;

        await _teamRepository.UpdateAsync(team);
        return team;
    }


    public async Task AddMemberAsync(Member member)
    {
        await _teamRepository.AddMemberAsync(member);
    }

    public async Task<Team?> GetTeamByMemberId(Guid memberId)
    {
        return await _teamRepository.GetTeamByMemberId(memberId);
    }


    public async Task<List<MemberDto>> GetMembersWithTasksByTeamIdAsync(Guid teamId)
    {
        var members = await _teamRepository.GetMembersWithTasksByTeamIdAsync(teamId);
        return _mapper.Map<List<MemberDto>>(members);
    }

    public async Task<Member?> GetMemberByUserIdAsync(Guid userId, Guid teamId)
    {
        return await _teamRepository.GetMemberByUserIdAsync(userId, teamId);
    }

    public Task<string?> GetUserFullNameByMemberIdAsync(Guid? memberId)
    {
        return _teamRepository.GetUserFullNameByMemberIdAsync(memberId);
    }

    public async Task SendTeamInviteAsync(SendTeamInviteDto dto)
    {
        var invitation = await _teamRepository.CreateTeamInvitationAsync(dto.TeamId, dto.Email);

        // Prepare and send email
        var inviteLink = $"https://yourapi.com/api/team/accept-invite?token={invitation.Token}";
        var subject = "You're Invited to Join a Team!";
        var body = $"Hello, You’ve been invited to join a team. Click <a href='{inviteLink}'>here</a> to accept the invite";

        await _emailService.SendEmailAsync(dto.Email, subject, body);
    }

    public async Task<TeamInvitation?> GetInvitationByTokenAsync(string token)
    {
        return await _teamRepository.GetInvitationByTokenAsync(token);
    }

    public Task UpdateInvitationAsync(TeamInvitation invitation)
    {
        return _teamRepository.UpdateInvitationAsync(invitation);
    }

    public async Task<Member?> GetMemberByIdAsync(Guid memberId)
    {
       return await _teamRepository.GetMemberByIdAsync(memberId);
    }
}
