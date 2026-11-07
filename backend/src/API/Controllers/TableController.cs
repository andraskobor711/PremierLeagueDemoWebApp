using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueWebApi.API.Schemas.Table;
using PremierLeagueWebApi.Core.Managers;
using PremierLeagueWebApi.Core.Security;
using System.Reflection.Metadata.Ecma335;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TableController(ITableManager _tableManager, ISeasonManager _seasonManager) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = AuthorizationClaims.CanReadTables)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TableSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<TableSchema>> GetTableBySeason([FromQuery] int? seasonId)
    {
        var usedSeasonId = seasonId
            ?? (await _seasonManager.GetCurrentSeasonBasicInfo())?.Id;

        if (usedSeasonId is null)
        {
            return NotFound();
        }

        var table = await _tableManager.GetTableBySeason(usedSeasonId.Value);

        return Ok(table.Adapt<TableSchema>());
    }

    [HttpGet]
    [Route("current")]
    [Authorize(Policy = AuthorizationClaims.CanReadTables)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TableSchema), StatusCodes.Status200OK)]
    public async Task<ActionResult<TableSchema>> GetCurrentSeasonTable()
    {
        var currentSeason = await _seasonManager.GetCurrentSeasonBasicInfo();
        if (currentSeason is null)
        {
            return NotFound();
        }

        var table = await _tableManager.GetTableBySeason(currentSeason.Id);

        return Ok(table.Adapt<TableSchema>());
    }
}
