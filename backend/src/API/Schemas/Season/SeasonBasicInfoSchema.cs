namespace PremierLeagueWebApi.API.Schemas.Season;

public record SeasonBasicInfoSchema
{
    public int Id { get; init; }

    public required string Label { get; init; }
}
