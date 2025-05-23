
using TaskManager.Application.DTOs.Task;

namespace TaskManager.Application.DTOs.Account;

public class LoginResponseDTO
{
    public UserDTO User { get; set; }
    public List<TaskDto> AssignedTasks { get; set; } = new();
}
