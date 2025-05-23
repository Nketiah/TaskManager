

using TaskManager.Application.DTOs.Member;
using TaskManager.Application.DTOs.Team;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Teams
{
    public interface ITeamService
    {
        Task<TeamDto> CreateTeamAsync(Team request);
        Task<TeamDto> GetTeamByIdAsync(Guid teamId);
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
        Task<Team?> UpdateTeamAsync(Guid teamId, UpdateTeamRequestDto request);
        Task<bool> DeleteTeamAsync(Guid teamId);
        Task AddMemberAsync(Member member);
        Task<Team?> GetTeamByMemberId(Guid memberId);
        Task<List<MemberDto>> GetMembersWithTasksByTeamIdAsync(Guid teamId);
        Task<Member?> GetMemberByUserIdAsync(Guid userId, Guid teamId);
        Task<string?> GetUserFullNameByMemberIdAsync(Guid? memberId);
        Task SendTeamInviteAsync(SendTeamInviteDto dto);
        Task<TeamInvitation?> GetInvitationByTokenAsync(string token);
        Task UpdateInvitationAsync(TeamInvitation invitation);
        Task<Member?> GetMemberByIdAsync(Guid memberId);
    }
}

