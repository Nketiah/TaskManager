
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tasks;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskItem?> GetTaskByIdAsync(Guid taskId)
    {
        return await _taskRepository.GetByIdAsync(taskId);
    }

    public async Task<List<TaskItem>> GetTasksByTeamIdAsync(Guid teamId)
    {
        return await _taskRepository.GetAllByTeamIdAsync(teamId);
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task)
    {
        await _taskRepository.AddAsync(task);
        return task;
    }

    public async Task<TaskItem?> UpdateTaskAsync(TaskItem task)
    {
        var existing = await _taskRepository.GetByIdAsync(task.Id);
        if (existing == null)
            return null;

        // Optionally update fields individually here
        await _taskRepository.UpdateAsync(task);
        return task;
    }

    public async Task<bool> DeleteTaskAsync(Guid taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            return false;

        await _taskRepository.DeleteAsync(task);
        return true;
    }

}
