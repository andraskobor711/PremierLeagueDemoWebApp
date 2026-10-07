using Microsoft.AspNetCore.Mvc;
using Mapster;
using PremierLeagueWebApi.BLL.Repositories;
using PremierLeagueWebApi.Models;
using PremierLeagueWebApi.API.Schemas.Team;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TeamsController(ITeamRepository _teamRepository) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<List<TeamSchema>>> GetAllTeams()
    {
        List<Team> teams = [.. await _teamRepository.GetAll()];

        return Ok(teams.Adapt<List<TeamSchema>>());
    }

    [HttpGet]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<TeamSchema>> GetTeamById([FromRoute] int id)
    {
        Team? team = await _teamRepository.GetTeamById(id);

        return team is not null
            ? Ok(team.Adapt<TeamSchema>())
            : NotFound();
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<TeamSchema>> Create([FromBody]CreateTeamSchema createTeamSchema)
    {

        var newTeam = createTeamSchema.Adapt<Team>();
        var newTeamId = await _teamRepository.Add(newTeam);

        var createdTeam = await _teamRepository.GetTeamById(newTeamId);

        return Ok(createdTeam.Adapt<TeamSchema>());
    }

    [HttpPut]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<TeamSchema>> Update([FromRoute] int id, [FromBody]UpdateTeamSchema updateTeamSchema)
    {

        var teamToUpdate = updateTeamSchema.Adapt<Team>();
        teamToUpdate.Id = id;

        var updatedTeam = await _teamRepository.Update(teamToUpdate);

        return updatedTeam is not null
            ? Ok(updatedTeam.Adapt<TeamSchema>())
            : NotFound();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        _ = await _teamRepository.Delete(id);

        return NoContent();
    }
}
