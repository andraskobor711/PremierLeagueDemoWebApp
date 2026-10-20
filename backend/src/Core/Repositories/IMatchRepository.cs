using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Repositories;

public interface IMatchRepository
{
    Task<IEnumerable<Match>> GetAll(CancellationToken? cancellationToken = default);

    Task<Match?> GetById(int id, CancellationToken? cancellation = default);

    Task<IEnumerable<Match>> GetByGameWeek(int seasonId, int gameWeek, CancellationToken? cancellationToken = default);

    Task<int> Add(Match match, CancellationToken? cancellationToken = default);

    Task<Match?> Update(Match matchUpdate, CancellationToken? cancellationToken = default);

    Task<bool> Delete(int id, CancellationToken? cancellationToken = default);
}
