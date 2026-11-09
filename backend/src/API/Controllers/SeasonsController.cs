using Microsoft.AspNetCore.Authorization;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueWebApi.API.Schemas.Season;
using PremierLeagueWebApi.Core.Managers;
using PremierLeagueWebApi.Core.Security;
using PremierLeagueWebApi.API.Schemas.Team;
using PremierLeagueWebApi.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SeasonsController(ISeasonManager _seasonManager) : Controller
{
    [HttpGet]
    [Authorize(Policy = AuthorizationClaims.CanReadSeasons)]
    [ProducesResponseType(typeof(ICollection<SeasonBasicInfoSchema>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<SeasonBasicInfoSchema>>> GetAllSeasons()
    {
        var currentSeason = await _seasonManager.GetAllSeasonBasicInfos();

        return Ok(currentSeason.Adapt<List<SeasonBasicInfoSchema>>());
    }

    [HttpGet]
    [Route("current")]
    [Authorize(Policy = AuthorizationClaims.CanReadSeasons)]
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
    [Authorize(Policy = AuthorizationClaims.CanReadSeasons)]
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

    [HttpPost]
    [Authorize(Policy = AuthorizationClaims.CanEditSeasons)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(SeasonBasicInfoSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<SeasonBasicInfoSchema>> CreateSeason(
        [FromBody] CreateSeasonSchema createSeasonSchema)
    {
        var createdSeasonBasicInfo = await _seasonManager
            .AddNewSeasonBasicInfo(createSeasonSchema.Adapt<SeasonBasicInfo>());

        return Ok(createdSeasonBasicInfo.Adapt<SeasonBasicInfoSchema>());
    }

    [HttpGet]
    [Route("{id}/teams")]
    [Authorize(Policy = AuthorizationClaims.CanReadSeasons)]
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

    [HttpPut]
    [Route("{id}/teams")]
    [Authorize(Policy = AuthorizationClaims.CanEditSeasons)]
    [Authorize(Policy = AuthorizationClaims.CanReadTeams)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(SeasonSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<SeasonSchema>> UpdateTeamsOfSeason(
        [FromRoute] int id,
        [FromBody] TeamsOfSeasonSchema teamsOfSeason)
    {
        var basicSeasonupdate = teamsOfSeason.Adapt<RawSeasonTeamsUpdate>();
        var updatedSeason = await _seasonManager.UpdateSeasonTeams(id, basicSeasonupdate);
        if (updatedSeason is null)
        {
            return NotFound();
        }

        return Ok(updatedSeason.Adapt<SeasonSchema>());
    }
}
