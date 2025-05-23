using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Shared;
using TaskManager.Application.Account;
using TaskManager.Application.DTOs.Member;
using TaskManager.Application.DTOs.Team;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Teams;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;




namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IAccountService _accountService;
    protected APIResponse _response;
    private readonly IMapper _mapper;

    public TeamsController(ITeamService teamService, IMapper mapper, IAccountService accountService)
    {
        _teamService = teamService;
        _accountService = accountService;
        _response = new();
        _mapper = mapper;
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetAll()
    {
        try
        {
            var teamList = await _teamService.GetAllTeamsAsync();
            _response.Result = _mapper.Map<List<TeamDto>>(teamList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }





    [HttpGet("{id:guid}", Name = "GetById")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetById(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<TeamDto>(team);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {

            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }



    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateTeam([FromBody] CreateTeamRequestDto request)
    {
        try
        {
            if (request == null)
                return BadRequest("Invalid request payload.");

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid ownerId))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("User ID not found in token.");
                return Unauthorized(_response);
            }

            var team = new Team
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = ownerId
            };

            var createdTeam = await _teamService.CreateTeamAsync(team);

            _response.Result = _mapper.Map<TeamDto>(createdTeam);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetById", new { id = createdTeam.Id }, _response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages = new List<string> { ex.Message };
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }





[HttpPut("{id}")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<APIResponse>> UpdateTeam(Guid id, [FromBody] UpdateTeamRequestDto request)
{
    try
    {
        if (request == null || id != request.Id)
        {
            return BadRequest(request);
        }

        var updatedTeam = await _teamService.UpdateTeamAsync(id, request);
        if (updatedTeam == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.ErrorMessages = new List<string> { "Team not found or could not be updated." };
            return NotFound(_response);
        }

        _response.Result = _mapper.Map<TeamDto>(updatedTeam);
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }
    catch (Exception ex)
    {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string> { ex.ToString() };
    }

    return _response;
}


   


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> DeleteTeam(Guid id)
    {
        try
        {
            if(id == Guid.Empty)
            {
                return BadRequest(id);
            }

            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            await _teamService.DeleteTeamAsync(id);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {

            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }



    [HttpGet("{teamId}/members-with-tasks")]
    public async Task<ActionResult<APIResponse>> GetMembersWithTasks(Guid teamId)
    {
        var members = await _teamService.GetMembersWithTasksByTeamIdAsync(teamId);

        _response.Result = members;
        _response.StatusCode = HttpStatusCode.OK;

        return Ok(_response);
    }


    [HttpPost("add-member")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> AddMemberToTeam([FromBody] AddMemberRequestDto request)
    {
        try
        {
            var existingMember = await _teamService.GetMemberByUserIdAsync(request.UserId, request.TeamId);
            if (existingMember != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("User is already a member of this team.");
                return BadRequest(_response);
            }

            var team = await _teamService.GetTeamByIdAsync(request.TeamId);
            if (team == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Team not found.");
                return NotFound(_response);
            }

            var member = new Member
            {
                TeamId = request.TeamId,
                UserId = request.UserId,
                Email = request.Email,
                Role = request.Role
            };

            await _teamService.AddMemberAsync(member);

            // Map to DTO to avoid circular reference issues
            var memberDto = new MemberDto
            {
                Id = member.Id,
                TeamId = member.TeamId,
                UserId = member.UserId,
                Email = member.Email,
                Role = member.Role
            };

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = memberDto;

            return CreatedAtAction(nameof(AddMemberToTeam), new { id = member.Id }, _response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }


    [HttpPost("invite")]
    public async Task<IActionResult> SendInvite([FromBody] SendTeamInviteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest(new APIResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = new List<string> { "Email is required." }
            });
        }
        await _teamService.SendTeamInviteAsync(dto);
        return Ok(new APIResponse
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Result = "Invitation email sent successfully."
        });
    }



    [HttpGet("accept-invite")]
    public async Task<IActionResult> AcceptInvite([FromQuery] string token)
    {
        var response = new APIResponse();

        try
        {
            var invitation = await _teamService.GetInvitationByTokenAsync(token);

            if (invitation == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessages.Add($"Invitation with token '{token}' was not found.");
                return NotFound(response);
            }

            // Check expiration
            var isExpired = invitation.CreatedAt.AddHours(48) < DateTime.UtcNow;
            if (isExpired)
            {
                invitation.Status = InvitationStatus.Expired;
                await _teamService.UpdateInvitationAsync(invitation);

                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Invitation has expired.");
                return BadRequest(response);
            }

            if (invitation.Status != InvitationStatus.Pending)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Invitation is no longer valid.");
                return BadRequest(response);
            }

            // Mark as accepted
            invitation.Status = InvitationStatus.Accepted;
            invitation.RespondedAt = DateTime.UtcNow;
            await _teamService.UpdateInvitationAsync(invitation);

            var user = await _accountService.GetUserByEmailAsync(invitation.Email);

            if (user == null || user.UserId == Guid.Empty)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessages.Add($"No user found with email '{invitation.Email}'.");
                return NotFound(response);
            }

            var member = new Member
            {
                TeamId = invitation.TeamId,
                Email = invitation.Email,
                UserId = user.UserId,
                JoinedAt = DateTime.UtcNow
            };

            await _teamService.AddMemberAsync(member);

            response.StatusCode = HttpStatusCode.OK;
            response.Result = "Invitation accepted and member added to the team.";
            return Ok(response);
        }
        catch (Exception)
        {
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.IsSuccess = false;
            response.ErrorMessages.Add($"An unexpected error occurred");
            return StatusCode((int)HttpStatusCode.InternalServerError, response);
        }
    }



}



// Team B id  51fb4149-65bb-450f-b8f3-c177064ae31f


