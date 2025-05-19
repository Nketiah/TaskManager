using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Team;
using TaskManager.Application.Teams;




namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    // GET: api/teams
    [HttpGet]
    public async Task<ActionResult<List<TeamDto>>> GetAll()
    {
        var teams = await _teamService.GetAllTeamsAsync();
        return Ok(teams);
    }

    // GET: api/teams/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDto>> GetById(Guid id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);
        if (team == null)
            return NotFound();

        return Ok(team);
    }



    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TeamDto>> CreateTeam([FromBody] CreateTeamRequestDto request)
    {
        var createdTeam = await _teamService.CreateTeamAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = createdTeam.Id }, createdTeam);
    }



    // PUT: api/teams/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTeam(Guid id, [FromBody] UpdateTeamRequestDto request)
    {
        var updated = await _teamService.UpdateTeamAsync(id, request);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/teams/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeam(Guid id)
    {
        var deleted = await _teamService.DeleteTeamAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

}
