using PremierLeagueWebApi.Models;

namespace PremierLeagueWebApi.BLL.Repositories;

public interface ITeamRepository
{
    Task<IEnumerable<Team>> GetAll(CancellationToken? cancellation = default);

    Task<Team?> GetTeamById(int id,CancellationToken? cancellation = default);
    
    Task<Team?> GetTeamByName(string name,CancellationToken? cancellation = default);

    Task<int> Add(Team team, CancellationToken? cancellation = default);

    Task<Team?> Update(Team team, CancellationToken? cancellation = default);

    Task<bool> Delete(int id, CancellationToken? cancellation = default);
}
