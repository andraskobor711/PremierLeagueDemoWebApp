using PremierLeagueWebApi.Core.Repositories;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public class SeasonManager(ISeasonRepository _seasonRepository) : ISeasonManager
{
    public async Task<Season?> GetCurrentSeason()
    {
        var month = DateTime.UtcNow.Month;
        var year = month <= 6 ? DateTime.UtcNow.Year - 1 : DateTime.UtcNow.Year;
        var seasonLabel = $"{year}/{year + 1}";

        var season = await _seasonRepository.GetSeasonByLabel(seasonLabel);
        if (season is not null)
        {

            season?.Teams = [.. season.Teams.OrderBy(t => t.UsedName)];
        }

        return season;
    }

    public async Task<Season?> GetSeasonById(int id)
    {
        var season = await _seasonRepository.GetSeasonById(id);
        season?.Teams = [.. season.Teams.OrderBy(t => t.UsedName)];

        return season;
    }
}
