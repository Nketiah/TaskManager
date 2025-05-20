using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly TaskManagerDbContext _db;

        public ActivityLogRepository(TaskManagerDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ActivityLog>> GetLogsForTeamAsync(Guid teamId)
        {
            return await _db.ActivityLogs
                .Where(log => log.TeamId == teamId)
                .OrderByDescending(log => log.Timestamp)
                .ToListAsync();
        }

        public async Task LogAsync(Guid teamId, string action, string performedBy, string? details = null)
        {
            var log = new ActivityLog
            {
                TeamId = teamId,
                Action = action,
                PerformedBy = performedBy,
                Details = details,
                Timestamp = DateTime.UtcNow
            };

            _db.ActivityLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}
