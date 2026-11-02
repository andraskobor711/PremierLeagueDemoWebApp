namespace PremierLeagueWebApi.Core.Security;

public static class AuthorizationClaims
{
    public const string CanReadTeams = nameof(CanReadTeams);
    public const string CanEditTeams = nameof(CanEditTeams);
    public const string CanReadMatches = nameof(CanReadMatches);
    public const string CanEditMatches = nameof(CanEditMatches);
    public const string CanReadTables = nameof(CanReadTables);

    public static IReadOnlyList<string> All =>
    [
        CanReadTeams,
        CanEditTeams,
        CanReadMatches,
        CanEditMatches,
        CanReadTables
    ];

    public static IReadOnlyList<string> ForRole(string role) => role switch
    {
        UserRoles.Admin => All,
        UserRoles.User =>
        [
            CanReadTeams,
            CanReadMatches,
            CanReadTables
        ],
        _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported user role.")
    };
}

public static class UserRoles
{
    public const string Admin = "admin";
    public const string User = "user";
}
