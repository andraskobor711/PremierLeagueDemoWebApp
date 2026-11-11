namespace PremierLeagueWebApi.API.Schemas.Auth;

public sealed record RoleClaimSchema
{
    public required string Type { get; init; }

    public required string Value { get; init; }
}
