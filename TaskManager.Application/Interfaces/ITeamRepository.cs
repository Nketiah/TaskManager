

using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id);
    Task<List<Team>> GetAllAsync();
    Task AddAsync(Team team);
    Task UpdateAsync(Team team);
    Task DeleteAsync(Team team);
    Task AddMemberAsync(Member member);
    Task<Team?> GetTeamByMemberId(Guid memberId);
    Task<List<Member>> GetMembersWithTasksByTeamIdAsync(Guid teamId);
    Task<Member?> GetMemberByUserIdAsync(Guid userId, Guid teamId);
    Task<string?> GetUserFullNameByMemberIdAsync(Guid? memberId);
    Task<TeamInvitation> CreateTeamInvitationAsync(Guid teamId, string email);
    Task<TeamInvitation?> GetInvitationByTokenAsync(string token);
    Task UpdateInvitationAsync(TeamInvitation invitation);
    Task<Member?> GetMemberByIdAsync(Guid memberId);
    Task<bool> IsTeamNameUniqueAsync(string name);
}
