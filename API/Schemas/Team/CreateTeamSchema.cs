namespace PremierLeagueWebApi.API.Schemas.Team;

public record CreateTeamSchema
{
    public required string FullName { get; init; }

    public required string UsedName { get; init; }

    public required string Abbreviation { get; init; }

    public string? OfficialWebsite { get; init; }
}
