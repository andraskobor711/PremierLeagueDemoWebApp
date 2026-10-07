using Mapster;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueWebApi.API.Schemas.Season;
using PremierLeagueWebApi.BLL.Repositories;
using PremierLeagueWebApi.Models;

namespace PremierLeagueWebApi.API.Controllers;

public class SeasonsController(ISeasonRepository _seasonRepository) : Controller
{
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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
}
