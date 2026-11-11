using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Models;

public record SeasonTeamsUpdate
{
    public IEnumerable<Team> Teams { get; set; } = [];
}
