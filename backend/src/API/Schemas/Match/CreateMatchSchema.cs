namespace PremierLeagueWebApi.API.Schemas.Match;

public record CreateMatchSchema
{
    public required int SeasonId { get; init; }

    public required int HomeTeamId { get; init; }

    public required int AwayTeamId { get; init; }

    public required int Matchweek { get; init; }

    public required DateTime KickoffAt { get; init; }

    public int? HomeScore { get; init; }

    public int? AwayScore { get; init; }
}
