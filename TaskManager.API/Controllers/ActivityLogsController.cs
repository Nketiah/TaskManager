using System.Net;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Shared;
using TaskManager.Application.Interfaces;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityLogsController : ControllerBase
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly APIResponse _apiResponse;
        public ActivityLogsController(IActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
            _apiResponse = new();
        }


        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetAllActivities(Guid teamId)
        {
            try
            {
                var logs = await _activityLogRepository.GetLogsForTeamAsync(teamId);
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = logs;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add("Failed to retrieve activity logs.");
                _apiResponse.ErrorMessages.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
        }


    }
}
