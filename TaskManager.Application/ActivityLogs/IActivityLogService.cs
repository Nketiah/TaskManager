

using TaskManager.Domain.Entities;

namespace TaskManager.Application.ActivityLogs
{
    public interface IActivityLogService
    {
        Task LogAsync(Guid teamId, string action, string performedBy, string? details = null);
        Task<IEnumerable<ActivityLog>> GetLogsForTeamAsync(Guid teamId);
    }
}
