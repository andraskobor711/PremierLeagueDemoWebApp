namespace PremierLeagueWebApi.API.Schemas.Team;

public record TeamInSeasonSchema
{
    public int Id { get; init; }

    public required string FullName { get; init; }

    public required string UsedName { get; init; }

    public required string Abbreviation { get; init; }

    public string? LogoSrc { get; init; }

    public string? OfficialWebsite { get; init; }
}
