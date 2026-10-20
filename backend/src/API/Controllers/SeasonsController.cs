using Mapster;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueWebApi.API.Schemas.Season;
using PremierLeagueWebApi.Core.Repositories;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SeasonsController(ISeasonRepository _seasonRepository) : Controller
{
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(SeasonSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(Season season)
    {
        if (!ModelState.IsValid)
        {
            return View(season);
        }

        var newSeasonId = await _seasonRepository.Add(season);
        var newSeason = await _seasonRepository.GetSeasonById(newSeasonId);

        return Ok(newSeason.Adapt<SeasonSchema>());
    }

    [HttpGet]
    [Route("current")]
    [ProducesResponseType(typeof(SeasonSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<SeasonSchema>> GetCurrentSeason()
    {
        // todo: implement logic to determine the current season based on the current date
        var currentSeason = await _seasonRepository.GetSeasonByLabel("2026/27");
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
        var season = await _seasonRepository.GetSeasonById(id);
        if (season == null)
        {
            return NotFound();
        }

        return Ok(season.Adapt<SeasonSchema>());
    }
}
