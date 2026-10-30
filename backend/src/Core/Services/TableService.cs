using PremierLeagueWebApi.Core.Models;
using PremierLeagueWebApi.Core.Repositories;

namespace PremierLeagueWebApi.Core.Services;

public class TableService(IMatchRepository _matchesRepository) : ITableService
{
    public async Task<ICollection<TableRowWithPosition>> GetSortedTableRowsBySeason(int seasonId)
    {
        // todo: teams involved in a certain season should be identified by the season, not by the registered games of the season
        Dictionary<int, TableRow> rows = [];
        var matches = await _matchesRepository.GetBySeason(seasonId);

        foreach (var match in matches)
        {
            if (!rows.TryGetValue(match.HomeTeamId, out var homeTeamRow))
            {
                homeTeamRow = new TableRow { Team = match.HomeTeam };
                rows.Add(match.HomeTeamId, homeTeamRow);
            }

            if (!rows.TryGetValue(match.AwayTeamId, out var awayTeamRow))
            {
                awayTeamRow = new TableRow { Team = match.AwayTeam };
                rows.Add(match.AwayTeamId, awayTeamRow);
            }

            if (match.HomeScore is not int homeScore || match.AwayScore is not int awayScore)
            {
                continue;
            }

            homeTeamRow.GoalsFor += homeScore;
            homeTeamRow.GoalsAgainst += awayScore;
            awayTeamRow.GoalsFor += awayScore;
            awayTeamRow.GoalsAgainst += homeScore;

            switch (homeScore.CompareTo(awayScore))
            {
                case > 0:
                    homeTeamRow.Wins++;
                    awayTeamRow.Losses++;
                    break;
                case 0:
                    homeTeamRow.Draws++;
                    awayTeamRow.Draws++;
                    break;
                case < 0:
                    homeTeamRow.Losses++;
                    awayTeamRow.Wins++;
                    break;
            }
        }

        List<TableRow> sortedRows = [.. rows.Values
            .OrderByDescending(row => row.Points)
            .ThenByDescending(row => row.GoalDifference)
            .ThenByDescending(row => row.GoalsFor)
            .ThenBy(row => row.Team.UsedName)];

        List<TableRowWithPosition> sortedRowsWithPositions = [];
        var position = 0;
        TableRow? previousRow = null;

        for (var index = 0; index < sortedRows.Count; index++)
        {
            var row = sortedRows[index];
            if (previousRow is null
                || row.Points != previousRow.Points
                || row.GoalsFor != previousRow.GoalsFor
                || row.GoalsAgainst != previousRow.GoalsAgainst)
            {
                position = index + 1;
            }

            sortedRowsWithPositions.Add(new TableRowWithPosition
            {
                Team = row.Team,
                Wins = row.Wins,
                Draws = row.Draws,
                Losses = row.Losses,
                GoalsFor = row.GoalsFor,
                GoalsAgainst = row.GoalsAgainst,
                Position = position
            });

            previousRow = row;
        }

        return sortedRowsWithPositions;
    }
}
