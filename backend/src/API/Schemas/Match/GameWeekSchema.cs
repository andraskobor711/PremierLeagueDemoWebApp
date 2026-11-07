using PremierLeagueWebApi.API.Schemas.Season;

namespace PremierLeagueWebApi.API.Schemas.Match;

public class GameWeekSchema
{
    public required SeasonBasicInfoSchema Season { get; set; }
    public int Number { get; set; }
}
