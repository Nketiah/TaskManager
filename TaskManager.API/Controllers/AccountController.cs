using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Shared;
using TaskManager.Application.Account;
using TaskManager.Application.DTOs.Account;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Interfaces;




namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        protected APIResponse _apiResponse;

        public AccountController(IAccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
            _apiResponse = new();
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var (result, errors) = await _accountService.RegisterAsync(request);

                if (!result.IsSuccess)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessages = errors.Any() ? errors : new List<string> { "User registration failed." };
                    return BadRequest(_apiResponse);
                }

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Result = result;
                return StatusCode((int)HttpStatusCode.Created, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add("An unexpected error occurred.");
                _apiResponse.ErrorMessages.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var loginResponse = await _accountService.LoginAsync(request);

            if (loginResponse == null || loginResponse.User == null || !loginResponse.User.IsSuccess)
            {
                return Unauthorized(new { Message = "Invalid login credentials." });
            }

            //await _emailService.SendEmailAsync(
            //    toEmail: "nketiahjoseph1@gmail.com",
            //    subject: "Login Notification",
            //    body: $"Hello {loginResponse.User.FullName},\n\nYou have successfully logged in.\n\nBest regards,\nTaskManager Team"
            //);
            return Ok(loginResponse);
            // pwzn olgu yqls mpmv
        }



        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var user = HttpContext.User.Identity;
            Console.WriteLine($"User Authenticated: {user?.IsAuthenticated}");
            Console.WriteLine($"User Name: {user?.Name}");

            await _accountService.LogoutAsync();
            return NoContent();
        }


        [HttpGet("user-tasks")]
        [Authorize]
        public async Task<IActionResult> GetUserTasks()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("User ID not found or invalid in claims.");
            }

            var tasks = await _accountService.GetTasksForUserAsync(userId);
            var taskDtos = tasks.Select(t => new TaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                CreatedAt = t.CreatedAt,
                AssignedToEmail = t.AssignedTo?.Email
            }).ToList();
            return Ok(taskDtos);
        }


    }
}
