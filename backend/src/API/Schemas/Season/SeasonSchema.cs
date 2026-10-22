namespace PremierLeagueWebApi.API.Schemas.Season;

public record SeasonSchema
{
    public int Id { get; init; }

    public required string Label { get; init; }
}
