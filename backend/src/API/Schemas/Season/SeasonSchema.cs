using PremierLeagueWebApi.API.Schemas.Team;

namespace PremierLeagueWebApi.API.Schemas.Season;

public record SeasonSchema
{
    public int Id { get; init; }

    public required string Label { get; init; }

    public ICollection<TeamSchema> Teams { get; init; } = [];
}
