namespace PremierLeagueWebApi.Core.Models;

public class GameWeek
{
    public required SeasonBasicInfo SeasonBasicInfo { get; set; }

    public int Number { get; set; }
}
