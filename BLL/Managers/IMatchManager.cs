using PremierLeagueWebApi.BLL.Dto;
using PremierLeagueWebApi.Models;

namespace PremierLeagueWebApi.BLL.Managers;

public interface IMatchManager
{
    public Task<IEnumerable<Match>> GetAllMatches(CancellationToken? cancellationToken = default);

    public Task<Match?> GetMatchById(int id, CancellationToken? cancellationToken = default);

    public Task<Match?> AddNewMatch(CreateMatchDto createMatchDto, CancellationToken? cancellationToken = default);

    public Task<Match?> UpdateMatch(UpdateMatchDto updateMatchDto, CancellationToken? cancellationToken = default);

    public Task<bool> DeleteMatch(int id, CancellationToken? cancellationToken = default);
}
