using PremierLeagueWebApi.Core.Models;
using PremierLeagueWebApi.Core.Repositories;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public class SeasonManager(ISeasonRepository _seasonRepository) : ISeasonManager
{
    public async Task<ICollection<SeasonBasicInfo>> GetAllSeasonBasicInfos()
        => [.. await _seasonRepository.GetAllSeasonBasicInfos()];

    public async Task<SeasonBasicInfo?> GetCurrentSeasonBasicInfo()
    {
        var month = DateTime.UtcNow.Month;
        var year = month <= 6 ? DateTime.UtcNow.Year - 1 : DateTime.UtcNow.Year;
        var seasonLabel = $"{year}/{year + 1}";

        return await _seasonRepository.GetSeasonBasicInfoByLabel(seasonLabel);
    }

    public async Task<SeasonBasicInfo?> GetSeasonBasicInfoById(int id)
        => await _seasonRepository.GetSeasonBasicInfoById(id);

    public async Task<ICollection<Team>?> GetTeamsOfSeason(int id)
        => await _seasonRepository.GetTeamsOfSeason(id);
}
