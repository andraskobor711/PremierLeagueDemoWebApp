using Microsoft.AspNetCore.Mvc;
using Mapster;
using PremierLeagueWebApi.API.Schemas.Match;
using PremierLeagueWebApi.Core.Managers;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchesController(IMatchManager _matchManager) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<MatchSchema>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MatchSchema>>> GetAllMatches()
    {
        List<Match> matches = [.. await _matchManager.GetAllMatches()];

        return Ok(matches.Adapt<List<MatchSchema>>());
    }

    [HttpGet]
    [Route("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(MatchSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<MatchSchema>> GetMatchById([FromRoute] int id)
    {
        Match? match = await _matchManager.GetMatchById(id);

        return match is not null
            ? Ok(match.Adapt<MatchSchema>())
            : NotFound();
    }

    [HttpGet]
    [Route("by-gameweek")]
    [Produces("application/json")]
    public async Task<ActionResult<List<MatchSchema>>> GetMatchesByGameWeek(
        [FromQuery] int seasonId,
        [FromQuery] int gameWeek)
    {
        List<Match> matches = [.. await _matchManager.GetMatchesByGameWeek(seasonId, gameWeek)];
        return Ok(matches.Adapt<List<MatchSchema>>());
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(MatchSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<MatchSchema>> Create([FromBody] CreateMatchSchema createMatchSchema)
    {
        var createdMatch = await _matchManager.AddNewMatch(createMatchSchema.Adapt<Match>());

        return Ok(createdMatch.Adapt<MatchSchema>());
    }

    [HttpPut]
    [Route("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(MatchSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<MatchSchema>> Update([FromRoute] int id, UpdateMatchSchema updateMatchSchema)
    {

        var matchToUpdate = updateMatchSchema.Adapt<Match>();
        matchToUpdate.Id = id;
        var updatedMatch = await _matchManager.UpdateMatch(matchToUpdate);

        return updatedMatch is not null
            ? Ok(updatedMatch.Adapt<MatchSchema>())
            : NotFound();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        _ = await _matchManager.DeleteMatch(id);

        return NoContent();
    }
}
