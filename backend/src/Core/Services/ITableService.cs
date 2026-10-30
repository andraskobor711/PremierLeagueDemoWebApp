using PremierLeagueWebApi.Core.Models;

namespace PremierLeagueWebApi.Core.Services;

public interface ITableService
{
    Task<ICollection<TableRowWithPosition>> GetSortedTableRowsBySeason(int seasonId);
}
