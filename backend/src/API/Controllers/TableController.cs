using Mapster;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueWebApi.API.Schemas.Table;
using PremierLeagueWebApi.Core.Managers;
using PremierLeagueWebApi.Core.Models;
using PremierLeagueWebApi.Core.Services;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TableController(ITableManager _tableManager, ISeasonManager _seasonManager) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TableSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<TableSchema>> GetTableBySeason([FromQuery] int? seasonId)
    {
        var usedSeasonId = seasonId
            ?? (await _seasonManager.GetCurrentSeason()).Id;

        var table = await _tableManager.GetTableBySeason(usedSeasonId);

        return Ok(table.Adapt<TableSchema>());
    }

    [HttpGet]
    [Route("current")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TableSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<TableSchema>> GetCurrentSeasonTable()
    {
        var currentSeason = await _seasonManager.GetCurrentSeason();

        var table = await _tableManager.GetTableBySeason(currentSeason.Id);

        return Ok(table.Adapt<TableSchema>());
    }
}
