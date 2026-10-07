using Microsoft.AspNetCore.Mvc;
using Mapster;
using PremierLeagueWebApi.API.Schemas.Match;
using PremierLeagueWebApi.Core.Dto;
using PremierLeagueWebApi.Core.Managers;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchesController(IMatchManager _matchManager) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<List<MatchSchema>>> GetAllMatches()
    {
        List<Match> matches = [.. await _matchManager.GetAllMatches()];

        return Ok(matches.Adapt<List<MatchSchema>>());
    }

    [HttpGet]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<MatchSchema>> GetMatchById([FromRoute] int id)
    {
        Match? match = await _matchManager.GetMatchById(id);

        return match is not null
            ? Ok(match.Adapt<MatchSchema>())
            : NotFound();
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<MatchSchema>> Create([FromBody] CreateMatchSchema createMatchSchema)
    {

        var newMatchDto = createMatchSchema.Adapt<CreateMatchDto>();
        var createdMatch = await _matchManager.AddNewMatch(newMatchDto);

        return Ok(createdMatch.Adapt<MatchSchema>());
    }

    [HttpPut]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<MatchSchema>> Update([FromRoute] int id, UpdateMatchSchema updateMatchSchema)
    {

        var updateMatchDto = (id, updateMatchSchema).Adapt<UpdateMatchDto>();
        var updatedMatch = await _matchManager.UpdateMatch(updateMatchDto);

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
