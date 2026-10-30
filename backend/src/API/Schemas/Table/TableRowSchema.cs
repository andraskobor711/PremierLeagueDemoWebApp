namespace PremierLeagueWebApi.API.Schemas.Table;

public record TableRowSchema
{
    public int Position { get; set; }

    public required string TeamName { get; set; }

    public string? LogoSrc { get; set; }

    public int GamesPlayed { get; set; }

    public int Wins { get; set; }

    public int Draws { get; set; }

    public int Losses { get; set; }

    public int GoalsFor { get; set; }

    public int GoalsAgainst { get; set; }

    public int GoalDifference { get; set; }

    public int Points { get; set; }
}
