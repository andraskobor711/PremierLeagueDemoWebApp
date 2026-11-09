namespace PremierLeagueWebApi.Core.Models;

public record RawSeasonTeamsUpdate
{
    public int[] TeamIds { get; set; } = [];
}
