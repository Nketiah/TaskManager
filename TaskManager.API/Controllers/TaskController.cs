using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Application.Tasks;
using TaskManager.Domain.Entities;
using AutoMapper;
using TaskManager.API.Shared;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Teams;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.ActivityLogs;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ITeamService _teamService;
        private readonly IActivityLogService _activityLogService;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;

        public TaskController(ITaskService taskService, IMapper mapper, ITeamService teamService, IActivityLogService activityLogService)
        {
            _taskService = taskService;
            _teamService = teamService;
            _mapper = mapper;
            _response = new APIResponse();
            _activityLogService = activityLogService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetTaskById(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Task not found.");
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<TaskDto>(task);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }



        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<APIResponse>> GetTasksByTeamId(Guid teamId)
        {
            var tasks = await _taskService.GetTasksByTeamIdAsync(teamId);
            _response.Result = _mapper.Map<IEnumerable<TaskDto>>(tasks);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }



        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateTask([FromBody] CreateTaskRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.AssignedToMemberId == null || request.AssignedToMemberId == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Valid Member Id is required.");
                return BadRequest(_response);
            }

            var task = _mapper.Map<TaskItem>(request);
            task.TeamId = request.TeamId;

            var created = await _taskService.CreateTaskAsync(task);

            _response.Result = _mapper.Map<TaskDto>(created);
            _response.StatusCode = HttpStatusCode.Created;
            var userName = await _teamService.GetUserFullNameByMemberIdAsync(request.AssignedToMemberId);

            await _activityLogService.LogAsync(
            teamId: task.TeamId,
            action: "Create Task",
            performedBy: userName ?? "Some User", 
            details: $"Task '{task.Title}' was created."
);

            return CreatedAtAction(nameof(GetTaskById), new { id = created.Id }, _response);
        }




        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse>> UpdateTask(Guid id, [FromBody] UpdateTaskRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid || id != request.Id)
                    return BadRequest(ModelState);

                var existingTask = await _taskService.GetTaskByIdAsync(id);
                if (existingTask == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Task not found.");
                    return NotFound(_response);
                }

                _mapper.Map(request, existingTask);
                var updated = await _taskService.UpdateTaskAsync(existingTask);

                _response.Result = _mapper.Map<TaskDto>(updated);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (DbUpdateException dbEx)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("failed to update record");
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An unexpected error occurred: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }





        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse>> DeleteTask(Guid id)
        {
            var success = await _taskService.DeleteTaskAsync(id);
            if (!success)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Task not found.");
                return NotFound(_response);
            }

            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
