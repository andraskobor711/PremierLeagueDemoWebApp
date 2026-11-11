using Mapster;
using PremierLeagueWebApi.Core.Exceptions;
using PremierLeagueWebApi.Core.Models;
using PremierLeagueWebApi.Core.Repositories;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public class SeasonManager(
    ISeasonRepository _seasonRepository,
    ITeamRepository _teamsRepository) : ISeasonManager
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
        => (await _seasonRepository
                .GetTeamsOfSeason(id))
            ?.OrderBy(t => t.UsedName)
            ?.ToList()
        ?? null;

    public async Task<SeasonBasicInfo> AddNewSeasonBasicInfo(SeasonBasicInfo newSeasonBasicInfo)
    {
        var createdSeasonId = await _seasonRepository
            .AddSeason(newSeasonBasicInfo.Adapt<Season>());

        return (await _seasonRepository.GetSeasonBasicInfoById(createdSeasonId))!;
    }

    public async Task<Season?> UpdateSeasonTeams(int id, RawSeasonTeamsUpdate basicSeasonUpdate)
    {
        var teams = await _teamsRepository.GetTeamsByIds(basicSeasonUpdate.TeamIds);
        var notFoundTeamIds = basicSeasonUpdate.TeamIds.Except(teams.Select(t => t.Id));
        if (notFoundTeamIds.Any())
        {
            throw new ExpectedDataMissingException(
                $"Teams with ids can not be found: {string.Join(", ", notFoundTeamIds.Select(id => string.Format("'{0}'", id)))}");
        }

        return await _seasonRepository.UpdateSeasonTeams(
            id: id,
            seasonTeamsUpdate: new SeasonTeamsUpdate
            {
                Teams = teams
            });
    }
}
