using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Mapster;
using PremierLeagueWebApi.API.Schemas.Match;
using PremierLeagueWebApi.Core.Managers;
using PremierLeagueWebApi.Core.Security;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchesController(IMatchManager _matchManager) : ControllerBase
{
    [HttpGet]
    [Route("all")]
    [Authorize(Policy = AuthorizationClaims.CanReadMatches)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<MatchSchema>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MatchSchema>>> GetAllMatches()
    {
        List<Match> matches = [.. await _matchManager.GetAllMatches()];

        return Ok(matches.Adapt<List<MatchSchema>>());
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize(Policy = AuthorizationClaims.CanReadMatches)]
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
    [Authorize(Policy = AuthorizationClaims.CanReadMatches)]
    [Produces("application/json")]
    public async Task<ActionResult<List<MatchSchema>>> GetMatchesByGameWeek(
        [FromQuery] int seasonId,
        [FromQuery] int gameWeek)
    {
        // todo: create detailed query parameter validation
        List<Match> matches = [.. await _matchManager.GetMatchesByGameWeek(seasonId, gameWeek)];
        return Ok(matches.Adapt<List<MatchSchema>>());
    }

    [HttpGet]
    [Route("current-gameweek")]
    [Authorize(Policy = AuthorizationClaims.CanReadMatches)]
    [Produces("application/json")]
    public async Task<ActionResult<List<MatchSchema>>> GetCurrentGameWeekMatches()
    {
        List<Match> matches = [.. await _matchManager.GetCurrentGameWeekMatches()];
        return Ok(matches.Adapt<List<MatchSchema>>());
    }

    [HttpGet]
    [Route("gameweek/current")]
    [Authorize(Policy = AuthorizationClaims.CanReadMatches)]
    [Produces("application/json")]
    public async Task<ActionResult<GameWeekSchema>> GetCurrentGameWeek()
    {
        var currentGameWeek = await _matchManager.GetCurrentGameWeek();
        return Ok(currentGameWeek.Adapt<GameWeekSchema>());
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationClaims.CanEditMatches)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(MatchSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<MatchSchema>> Create([FromBody] CreateMatchSchema createMatchSchema)
    {
        var createdMatch = await _matchManager.AddNewMatch(createMatchSchema.Adapt<Match>());

        return Ok(createdMatch.Adapt<MatchSchema>());
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(Policy = AuthorizationClaims.CanEditMatches)]
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
    [Authorize(Policy = AuthorizationClaims.CanEditMatches)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        _ = await _matchManager.DeleteMatch(id);

        return NoContent();
    }
}
