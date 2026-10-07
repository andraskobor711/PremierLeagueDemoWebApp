using PremierLeagueWebApi.Models;

namespace PremierLeagueWebApi.BLL.Repositories;

public interface IMatchRepository
{
    Task<IEnumerable<Match>> GetAll(CancellationToken? cancellationToken = default);

    Task<Match?> GetById(int id, CancellationToken? cancellation = default);

    Task<int> Add(Match match, CancellationToken? cancellationToken = default);

    Task<Match?> Update(Match matchUpdate, CancellationToken? cancellationToken = default);

    Task<bool> Delete(int id, CancellationToken? cancellationToken = default);
}
