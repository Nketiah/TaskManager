
namespace TaskManager.Application.DTOs.Account;

public class LoginRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
