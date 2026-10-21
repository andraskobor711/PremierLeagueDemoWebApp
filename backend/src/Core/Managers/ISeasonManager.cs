using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public interface ISeasonManager
{
    Task<Season?> GetSeasonById(int id);

    Task<Season?> GetCurrentSeason();
}
