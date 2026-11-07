using PremierLeagueWebApi.Core.Models;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public interface ISeasonManager
{
    Task<ICollection<SeasonBasicInfo>> GetAllSeasonBasicInfos();

    Task<SeasonBasicInfo?> GetSeasonBasicInfoById(int id);

    Task<SeasonBasicInfo?> GetCurrentSeasonBasicInfo();

    Task<ICollection<Team>?> GetTeamsOfSeason(int id);
}
