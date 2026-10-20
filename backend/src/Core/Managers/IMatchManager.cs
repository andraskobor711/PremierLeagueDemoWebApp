using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public interface IMatchManager
{
    public Task<IEnumerable<Match>> GetAllMatches(CancellationToken? cancellationToken = default);

    public Task<Match?> GetMatchById(int id, CancellationToken? cancellationToken = default);

    public Task<IEnumerable<Match>> GetMatchesByGameWeek(int seasonId, int gameWeek, CancellationToken? cancellationToken = default);

    public Task<Match?> AddNewMatch(Match match, CancellationToken? cancellationToken = default);

    public Task<Match?> UpdateMatch(Match match, CancellationToken? cancellationToken = default);

    public Task<bool> DeleteMatch(int id, CancellationToken? cancellationToken = default);
}
