using PremierLeagueWebApi.Core.Models;

namespace PremierLeagueWebApi.Core.Managers;

public interface ITableManager
{
    Task<Table> GetTableBySeason(int seasonId);
}
