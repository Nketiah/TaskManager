

using TaskManager.Application.DTOs.Account;

namespace TaskManager.Application.Interfaces;

public interface IAccountRepository
{
    Task<(UserDTO User, List<string> Errors)> RegisterAsync(RegisterRequestDto request);
    Task<LoginResponseDTO> LoginAsync(LoginRequestDto request);
    Task LogoutAsync();
}
