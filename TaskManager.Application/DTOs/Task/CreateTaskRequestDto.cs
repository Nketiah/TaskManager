

namespace TaskManager.Application.DTOs.Task;

public class CreateTaskRequestDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? AssignedToMemberId { get; set; }
    public Guid TeamId { get; set; }
}
