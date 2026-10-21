using Mapster;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueWebApi.API.Schemas.Season;
using PremierLeagueWebApi.Core.Managers;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SeasonsController(ISeasonManager _seasonManager) : Controller
{
    [HttpGet]
    [Route("current")]
    [ProducesResponseType(typeof(SeasonSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<SeasonSchema>> GetCurrentSeason()
    {
        var currentSeason = await _seasonManager.GetCurrentSeason();
        if (currentSeason == null)
        {
            return NotFound();
        }

        return Ok(currentSeason.Adapt<SeasonSchema>());
    }

    [HttpGet]
    [Route("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(SeasonSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<SeasonSchema>> GetSeasonById([FromRoute] int id)
    {
        var season = await _seasonManager.GetSeasonById(id);
        if (season == null)
        {
            return NotFound();
        }

        return Ok(season.Adapt<SeasonSchema>());
    }
}
