using PremierLeagueWebApi.Core.Models;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Repositories;

public interface ISeasonRepository
{
    Task<IEnumerable<SeasonBasicInfo>> GetAllSeasonBasicInfos(
        CancellationToken? cancellationToken = default);

    Task<SeasonBasicInfo?> GetSeasonBasicInfoById(
        int id,
        CancellationToken? cancellationToken = default);

    Task<Season?> GetSeasonById(
        int id,
        CancellationToken? cancellationToken = default);

    Task<ICollection<Team>?> GetTeamsOfSeason(
        int seasonId,
        CancellationToken? cancellationToken = default);

    Task<SeasonBasicInfo?> GetSeasonBasicInfoByLabel(
        string label,
        CancellationToken? cancellationToken = default);

    Task<int> AddSeason(Season season, CancellationToken? cancellationToken = default);

    Task<Season?> UpdateSeasonTeams(
        int id,
        SeasonTeamsUpdate seasonTeamsUpdate,
        CancellationToken? cancellationToken = default);
}
