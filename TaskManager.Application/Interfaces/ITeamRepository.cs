

using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id);
    Task<List<Team>> GetAllAsync();
    Task AddAsync(Team team);
    Task UpdateAsync(Team team);
    Task DeleteAsync(Team team);
}
