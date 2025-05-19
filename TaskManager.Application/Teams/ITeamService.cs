

using TaskManager.Application.DTOs.Team;

namespace TaskManager.Application.Teams
{
    public interface ITeamService
    {
        Task<TeamDto> CreateTeamAsync(CreateTeamRequestDto request);
        Task<TeamDto> GetTeamByIdAsync(Guid teamId);
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
        Task<bool> UpdateTeamAsync(Guid teamId, UpdateTeamRequestDto request);
        Task<bool> DeleteTeamAsync(Guid teamId);
    }
}

