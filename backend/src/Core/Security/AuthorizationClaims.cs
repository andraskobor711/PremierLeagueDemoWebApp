namespace PremierLeagueWebApi.Core.Security;

public static class AuthorizationClaims
{
    public const string CanReadTeams = nameof(CanReadTeams);
    public const string CanEditTeams = nameof(CanEditTeams);
    public const string CanReadMatches = nameof(CanReadMatches);
    public const string CanEditMatches = nameof(CanEditMatches);
    public const string CanReadTables = nameof(CanReadTables);
    public const string CanReadSeasons = nameof(CanReadSeasons);
    public const string CanEditSeasons = nameof(CanEditSeasons);


    public static IReadOnlyList<string> All =>
    [
        CanReadTeams,
        CanEditTeams,
        CanReadMatches,
        CanEditMatches,
        CanReadTables,
        CanReadSeasons,
        CanEditSeasons
    ];
}

public static class UserRoles
{
    public const string Admin = "admin";
    public const string User = "user";
}
