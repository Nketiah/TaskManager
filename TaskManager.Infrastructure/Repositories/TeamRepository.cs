
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;



namespace TaskManager.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{

    private readonly TaskManagerDbContext _db;
    public TeamRepository(TaskManagerDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Team team)
    {
        await _db.Teams.AddAsync(team);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Team team)
    {
        _db.Teams.Remove(team);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Team>> GetAllAsync()
    {
        return await _db.Teams
                .Include(t => t.Members)
                .ToListAsync();
    }

    public async Task<Team?> GetByIdAsync(Guid id)
    {
        return await _db.Teams
               .Include(t => t.Members) 
               .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task UpdateAsync(Team team)
    {
        _db.Teams.Update(team);
        await _db.SaveChangesAsync();
    }

    public async Task AddMemberAsync(Member member)
    {
        await _db.Members.AddAsync(member);
        await _db.SaveChangesAsync();
    }

}
