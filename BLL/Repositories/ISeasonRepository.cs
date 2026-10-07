using PremierLeagueWebApi.Models;

namespace PremierLeagueWebApi.BLL.Repositories;

public interface ISeasonRepository
{
    Task<IEnumerable<Season>> GetAll(CancellationToken? cancellationToken = default);

    Task<Season?> GetSeasonById(int id, CancellationToken? cancellationToken = default);

    Task<Season?> GetSeasonByLabel(string label, CancellationToken? cancellationToken = default);

    Task<int> Add(Season season, CancellationToken? cancellationToken = default);
}
