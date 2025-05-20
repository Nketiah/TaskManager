
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskManagerDbContext _db;
    public TaskRepository(TaskManagerDbContext db)
    {
        _db = db;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid taskId)
    {
        return await _db.Tasks
                        .Include(t => t.AssignedTo) 
                        .Include(t => t.Team)
                        .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<List<TaskItem>> GetAllByTeamIdAsync(Guid teamId)
    {
        return await _db.Tasks
                        .Where(t => t.TeamId == teamId)
                        .Include(t => t.AssignedTo)
                        .ToListAsync();
    }

    public async Task AddAsync(TaskItem task)
    {
        await _db.Tasks.AddAsync(task);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _db.Tasks.Update(task);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskItem task)
    {
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
    }
}
