
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Member;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
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
                .ThenInclude(m => m.AssignedTasks)
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

    public async Task<Team?> GetTeamByMemberId(Guid memberId)
    {
        return await _db.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Members.Any(m => m.UserId == memberId));
    }

    public async Task<List<Member>> GetMembersWithTasksByTeamIdAsync(Guid teamId)
    {
        return await _db.Members
            .Where(m => m.TeamId == teamId)
            .Include(m => m.AssignedTasks)
            .ToListAsync();
    }

    public async Task<Member?> GetMemberByUserIdAsync(Guid userId, Guid teamId)
    {
        return await _db.Members
            .FirstOrDefaultAsync(m => m.UserId == userId && m.TeamId == teamId);
    }

    public async Task<string?> GetUserFullNameByMemberIdAsync(Guid? memberId)
    {
        return await _db.Members
            .Where(m => m.Id == memberId)
            .Join(_db.Users,
                  member => member.UserId,
                  user => user.Id,
                  (member, user) => user.FullName) // or user.Email
            .FirstOrDefaultAsync();
    }


    public async Task<TeamInvitation> CreateTeamInvitationAsync(Guid teamId, string email)
    {
        var token = Guid.NewGuid().ToString();

        var invitation = new TeamInvitation
        {
            TeamId = teamId,
            Email = email,
            Token = token,
            Status = InvitationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _db.TeamInvitations.AddAsync(invitation);
        await _db.SaveChangesAsync();

        return invitation;
    }


    public async Task<TeamInvitation?> GetInvitationByTokenAsync(string token)
    {
        return await _db.TeamInvitations.FirstOrDefaultAsync(i => i.Token == token);
    }

    public async Task UpdateInvitationAsync(TeamInvitation invitation)
    {
        _db.TeamInvitations.Update(invitation);
        await _db.SaveChangesAsync();
    }

    public async Task<Member?> GetMemberByIdAsync(Guid memberId)
    {
        return await _db.Members
            .Include(m => m.AssignedTasks)
            .FirstOrDefaultAsync(m => m.Id == memberId);
    }

    public Task<bool> IsTeamNameUniqueAsync(string name)
    {
        throw new NotImplementedException();
    }
}
