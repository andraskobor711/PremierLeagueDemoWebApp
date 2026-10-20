using PremierLeagueWebApi.Core.Repositories;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public class MatchManager(IMatchRepository _matchRepository) : IMatchManager
{
    public async Task<IEnumerable<Match>> GetAllMatches(CancellationToken? cancellationToken = null)
        => (await _matchRepository.GetAll(cancellationToken))
        .OrderBy(m => m.KickoffAt).ThenBy(m => m.HomeTeam.UsedName);

    public async Task<Match?> GetMatchById(int id, CancellationToken? cancellationToken = null)
        => await _matchRepository.GetById(id, cancellationToken);

    public async Task<IEnumerable<Match>> GetMatchesByGameWeek(int seasonId, int gameWeek, CancellationToken? cancellationToken = null)
        => (await _matchRepository
        .GetByGameWeek(seasonId, gameWeek, cancellationToken))
        .OrderBy(m => m.KickoffAt).ThenBy(m => m.HomeTeam.UsedName);

    public async Task<Match?> AddNewMatch(Match match, CancellationToken? cancellationToken = null)
    {
        await _matchRepository.Add(match, cancellationToken);
        return await _matchRepository.GetById(match.Id, cancellationToken);
    }

    public async Task<Match?> UpdateMatch(Match match, CancellationToken? cancellationToken = null)
    {
        await _matchRepository.Update(match, cancellationToken);
        return await _matchRepository.GetById(match.Id, cancellationToken);
    }

    public async Task<bool> DeleteMatch(int id, CancellationToken? cancellationToken = null)
        => await _matchRepository.Delete(id, cancellationToken);
}
