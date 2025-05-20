

using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tasks;

public interface ITaskService
{
    Task<TaskItem?> GetTaskByIdAsync(Guid taskId);
    Task<List<TaskItem>> GetTasksByTeamIdAsync(Guid teamId);
    Task<TaskItem> CreateTaskAsync(TaskItem task);
    Task<TaskItem?> UpdateTaskAsync(TaskItem task);
    Task<bool> DeleteTaskAsync(Guid taskId);
}
