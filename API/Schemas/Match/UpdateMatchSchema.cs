namespace PremierLeagueWebApi.API.Schemas.Match;

public record UpdateMatchSchema
{
    public required string SeasonLabel { get; init; }

    public required string HomeTeamName { get; init; }

    public required string AwayTeamName { get; init; }

    public required int Matchweek { get; init; }

    public required DateTimeOffset KickoffAt { get; init; }

    public int? HomeScore { get; init; }

    public int? AwayScore { get; init; }
}
