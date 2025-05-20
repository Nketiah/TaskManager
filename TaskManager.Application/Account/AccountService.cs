using TaskManager.Application.Account;
using TaskManager.Application.DTOs.Account;
using TaskManager.Application.Interfaces;

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
    }
}
