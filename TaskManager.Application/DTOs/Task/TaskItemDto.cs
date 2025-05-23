

namespace TaskManager.Application.DTOs.Task;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? AssignedToEmail { get; set; }
}
