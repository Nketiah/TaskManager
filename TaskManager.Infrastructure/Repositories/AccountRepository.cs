

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TaskManager.API.Shared;
using TaskManager.Application.DTOs.Account;
using TaskManager.Application.Interfaces;
using TaskManager.Infrastructure.Identity;


namespace TaskManager.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AccountRepository> _logger;

    public AccountRepository(
         UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         ILogger<AccountRepository> logger,
         IJwtTokenGenerator jwtTokenGenerator
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator;
    }




    public async Task<(UserDTO User, List<string> Errors)> RegisterAsync(RegisterRequestDto request)
    {
        var errors = new List<string>();

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            errors.Add("User already exists with this email.");
            return (new UserDTO
            {
                IsSuccess = false,
                Email = request.Email,
                FullName = request.FullName,
                Token = null,
                UserId = Guid.Empty
            }, errors);
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            FullName = request.FullName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            errors.AddRange(result.Errors.Select(e => e.Description));
            return (new UserDTO
            {
                IsSuccess = false,
                Email = user.Email!,
                FullName = user.FullName,
                UserId = Guid.Empty,
                Token = null
            }, errors);
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!);

        return (new UserDTO
        {
            IsSuccess = true,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Token = token
        }, errors);
    }


    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new LoginResponseDTO
            {
                User = new UserDTO
                {
                    IsSuccess = false,
                    UserId = Guid.Empty,
                    FullName = string.Empty,
                    Email = request.Email,
                    Token = null,
                }
            };
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!);

        return new LoginResponseDTO
        {
            User = new UserDTO
            {
                IsSuccess = true,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Token = token
            }
        };
    }


    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }


}
