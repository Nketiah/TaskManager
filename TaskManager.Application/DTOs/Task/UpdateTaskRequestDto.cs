

using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Task;

public class UpdateTaskRequestDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TStatus Status { get; set; }
    public Guid? AssignedToMemberId { get; set; }
}
