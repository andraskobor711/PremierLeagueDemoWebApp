namespace PremierLeagueWebApi.API.Schemas.Season;

public record TeamsOfSeasonSchema
{
    public IEnumerable<TeamOfSeasonSchema> Teams { get; set; } = [];
}
