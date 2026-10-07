namespace PremierLeagueWebApi.API.Schemas.Team;

public record UpdateTeamSchema
{
    public required string FullName { get; init; }

    public required string UsedName { get; init; }

    public required string Abbreviation { get; init; }

    public string? OfficialWebsite { get; init; }
}
