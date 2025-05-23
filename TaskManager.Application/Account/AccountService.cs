using TaskManager.Application.Account;
using TaskManager.Application.DTOs.Account;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<(UserDTO User, List<string> Errors)> RegisterAsync(RegisterRequestDto request)
        {
            return await _accountRepository.RegisterAsync(request);
        }


        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDto request)
        {
            return await _accountRepository.LoginAsync(request);
        }

        public async Task LogoutAsync()
        {
            await _accountRepository.LogoutAsync();
        }

        public Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            return _accountRepository.GetUserByEmailAsync(email);
        }

        public Task<List<TaskItem>> GetTasksForUserAsync(Guid userId)
        {
            return _accountRepository.GetTasksForUserAsync(userId);
        }
    }
}
