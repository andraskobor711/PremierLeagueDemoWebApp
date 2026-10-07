using PremierLeagueWebApi.API.Schemas.Season;

namespace PremierLeagueWebApi.API.Schemas.Team;

public record TeamSchema
{
    public int Id { get; init; }

    public required string FullName { get; init; }

    public required string UsedName { get; init; }

    public required string Abbreviation { get; init; }

    public string? OfficialWebsite { get; init; }

    public ICollection<SeasonSchema> Seasons { get; init; } = [];
}
