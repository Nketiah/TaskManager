
using TaskManager.Application.DTOs.Account;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Account;

public interface IAccountService
{
    Task<(UserDTO User, List<string> Errors)> RegisterAsync(RegisterRequestDto request);
    Task<LoginResponseDTO> LoginAsync(LoginRequestDto request);
    Task LogoutAsync();
    Task<UserDTO?> GetUserByEmailAsync(string email);
    Task<List<TaskItem>> GetTasksForUserAsync(Guid userId);
}
