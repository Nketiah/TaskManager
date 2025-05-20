using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Shared;
using TaskManager.Application.DTOs.Team;
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
    protected APIResponse _response;
    private readonly IMapper _mapper;

    public TeamsController(ITeamService teamService, IMapper mapper)
    {
        _teamService = teamService;
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
                return BadRequest(request);

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var emailClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Email);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid ownerId))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("User ID not found in token.");
                return Unauthorized(_response);
            }

            var email = emailClaim?.Value ?? "unknown@example.com";

            
            var team = new Team
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = ownerId
            };

            var createdTeam = await _teamService.CreateTeamAsync(team);

            
            var member = new Member
            {
                TeamId = createdTeam.Id,
                UserId = ownerId,
                Email = email,
                Role = MemberRole.Owner
            };

            
            await _teamService.AddMemberAsync(member);

            _response.Result = _mapper.Map<TeamDto>(createdTeam);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetById", new { id = createdTeam.Id }, _response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
            return _response;
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

}
