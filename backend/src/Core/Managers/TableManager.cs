using PremierLeagueWebApi.Core.Exceptions;
using PremierLeagueWebApi.Core.Models;
using PremierLeagueWebApi.Core.Services;

namespace PremierLeagueWebApi.Core.Managers;

public class TableManager(ISeasonManager _seasonManager, ITableService _tableService) : ITableManager
{
    public async Task<Table> GetTableBySeason(int seasonId)
    {
        var season = await _seasonManager.GetSeasonById(seasonId)
            ?? throw new ExpectedDataMissingException($"Season with id '{seasonId}' is missing.");

        var sortedTableRows = await _tableService.GetSortedTableRowsBySeason(seasonId);
        return new Table { Season = season, SortedRows = sortedTableRows };
    }
}
