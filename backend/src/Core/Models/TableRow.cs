using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Models;

public class TableRow
{
    public required Team Team { get; set; }

    public int GamesPlayed => Wins + Draws + Losses;

    public int Wins { get; set; }

    public int Draws { get; set; }

    public int Losses { get; set; }

    public int GoalsFor { get; set; }

    public int GoalsAgainst { get; set; }

    public int GoalDifference => GoalsFor - GoalsAgainst;

    public int Points => Wins * 3 + Draws;
}
