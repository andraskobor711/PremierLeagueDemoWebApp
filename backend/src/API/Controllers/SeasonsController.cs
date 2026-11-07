using Microsoft.AspNetCore.Authorization;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueWebApi.API.Schemas.Season;
using PremierLeagueWebApi.Core.Managers;
using PremierLeagueWebApi.Core.Security;
using PremierLeagueWebApi.API.Schemas.Team;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SeasonsController(ISeasonManager _seasonManager) : Controller
{
    [HttpGet]
    [Authorize(Policy = AuthorizationClaims.CanReadMatches)]
    [ProducesResponseType(typeof(ICollection<SeasonBasicInfoSchema>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<SeasonBasicInfoSchema>>> GetAllSeasons()
    {
        var currentSeason = await _seasonManager.GetAllSeasonBasicInfos();

        return Ok(currentSeason.Adapt<List<SeasonBasicInfoSchema>>());
    }

    [HttpGet]
    [Route("current")]
    [Authorize(Policy = AuthorizationClaims.CanReadMatches)]
    [ProducesResponseType(typeof(SeasonBasicInfoSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<SeasonBasicInfoSchema>> GetCurrentSeason()
    {
        var currentSeason = await _seasonManager.GetCurrentSeasonBasicInfo();
        if (currentSeason == null)
        {
            return NotFound();
        }

        return Ok(currentSeason.Adapt<SeasonBasicInfoSchema>());
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize(Policy = AuthorizationClaims.CanReadMatches)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(SeasonBasicInfoSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<SeasonBasicInfoSchema>> GetSeasonById([FromRoute] int id)
    {
        var season = await _seasonManager.GetSeasonBasicInfoById(id);
        if (season == null)
        {
            return NotFound();
        }

        return Ok(season.Adapt<SeasonBasicInfoSchema>());
    }

    [HttpGet]
    [Route("{id}/teams")]
    [Authorize(Policy = AuthorizationClaims.CanReadTeams)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ICollection<TeamSchema>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<TeamSchema>>> GetTeamsOfSeason([FromRoute] int id)
    {
        var teamsOfSeason = await _seasonManager.GetTeamsOfSeason(id);
        if (teamsOfSeason is null)
        {
            return NotFound();
        }

        return Ok(teamsOfSeason.Adapt<List<TeamSchema>>());
    }
}
