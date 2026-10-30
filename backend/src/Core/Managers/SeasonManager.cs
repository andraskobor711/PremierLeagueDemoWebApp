using PremierLeagueWebApi.Core.Exceptions;
using PremierLeagueWebApi.Core.Repositories;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public class SeasonManager(ISeasonRepository _seasonRepository) : ISeasonManager
{
    public async Task<Season> GetCurrentSeason()
    {
        var month = DateTime.UtcNow.Month;
        var year = month <= 6 ? DateTime.UtcNow.Year - 1 : DateTime.UtcNow.Year;
        var seasonLabel = $"{year}/{year + 1}";

        var currentSeason = (await _seasonRepository.GetSeasonByLabel(seasonLabel))
            ?? throw new ExpectedDataMissingException($"Season with label '{seasonLabel}' is missing.");

        return currentSeason;
    }

    public async Task<Season> GetSeasonById(int id)
    {
        var season = (await _seasonRepository.GetSeasonById(id))
            ?? throw new ExpectedDataMissingException($"Season with id '{id}' is missing.");

        return season;
    }
}
