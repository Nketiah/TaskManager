

using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface IActivityLogRepository
{
    Task LogAsync(Guid teamId, string action, string performedBy, string? details = null);
    Task<IEnumerable<ActivityLog>> GetLogsForTeamAsync(Guid teamId);
}
