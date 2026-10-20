using Microsoft.EntityFrameworkCore;
using PremierLeagueWebApi.Infrastructure;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Repositories;

public class MatchRepository(PlContext _plContext) : IMatchRepository
{
    public async Task<IEnumerable<Match>> GetAll(CancellationToken? cancellationToken = null)
        => await _plContext
            .Matches
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Season)
            .ToListAsync(cancellationToken ?? CancellationToken.None);

    public async Task<Match?> GetById(int id, CancellationToken? cancellationToken)
        => await _plContext
            .Matches
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Season)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken ?? CancellationToken.None);

    public async Task<IEnumerable<Match>> GetByGameWeek(int seasonId, int gameWeek, CancellationToken? cancellationToken = null)
        => await _plContext
            .Matches
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Season)
            .Where(m => m.SeasonId == seasonId && m.Matchweek == gameWeek)
            .ToListAsync(cancellationToken ?? CancellationToken.None);

    public async Task<int> Add(Match match, CancellationToken? cancellationToken = null)
    {
        //todo: there should be constraints for new games, e.g. a team cannot be involved for two games on the same Matchweek
        _plContext.Matches.Add(match);
        await _plContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

        return match.Id;
    }

    public async Task<Match?> Update(Match matchUpdate, CancellationToken? cancellationToken)
    {
        var existingMatch = await _plContext.Matches.FindAsync(
            [matchUpdate.Id],
            cancellationToken ?? CancellationToken.None);

        if (existingMatch is null)
        {
            return null;
        }

        existingMatch.HomeTeamId = matchUpdate.HomeTeamId;
        existingMatch.AwayTeamId = matchUpdate.AwayTeamId;
        existingMatch.SeasonId = matchUpdate.SeasonId;
        existingMatch.Matchweek = matchUpdate.Matchweek;
        existingMatch.KickoffAt = matchUpdate.KickoffAt;
        existingMatch.HomeScore = matchUpdate.HomeScore;
        existingMatch.AwayScore = matchUpdate.AwayScore;

        await _plContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

        return existingMatch;
    }

    public async Task<bool> Delete(int id, CancellationToken? cancellationToken)
    {
        var matchToDelete = await _plContext
            .Matches
            .FirstOrDefaultAsync(m => m.Id == id);

        if (matchToDelete is null)
        {
            return false;
        }

        _plContext.Matches.Remove(matchToDelete);
        await _plContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

        return true;
    }
}
