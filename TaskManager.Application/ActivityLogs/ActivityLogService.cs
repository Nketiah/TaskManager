using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.ActivityLogs
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _activityLogRepo;
        private readonly ILogger<ActivityLogService> _logger;
        private readonly IMapper _mapper;

        public ActivityLogService(IActivityLogRepository activityLogRepo, ILogger<ActivityLogService> logger, IMapper mapper)
        {
            _activityLogRepo = activityLogRepo;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task LogAsync(Guid teamId, string action, string performedBy, string? details = null)
        {
            try
            {
                await _activityLogRepo.LogAsync(teamId, action, performedBy, details);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging activity for TeamId: {TeamId}", teamId);
                throw;
            }
        }

        public async Task<IEnumerable<ActivityLog>> GetLogsForTeamAsync(Guid teamId)
        {
            try
            {
                return await _activityLogRepo.GetLogsForTeamAsync(teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving activity logs for TeamId: {TeamId}", teamId);
                throw;
            }
        }
    }
}
