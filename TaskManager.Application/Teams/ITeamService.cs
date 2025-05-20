

using TaskManager.Application.DTOs.Team;
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
    }
}

