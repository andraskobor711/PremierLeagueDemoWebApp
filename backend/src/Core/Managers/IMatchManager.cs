using PremierLeagueWebApi.Core.Dto;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public interface IMatchManager
{
    public Task<IEnumerable<Match>> GetAllMatches(CancellationToken? cancellationToken = default);

    public Task<Match?> GetMatchById(int id, CancellationToken? cancellationToken = default);

    public Task<Match?> AddNewMatch(CreateMatchDto createMatchDto, CancellationToken? cancellationToken = default);

    public Task<Match?> UpdateMatch(UpdateMatchDto updateMatchDto, CancellationToken? cancellationToken = default);

    public Task<bool> DeleteMatch(int id, CancellationToken? cancellationToken = default);
}
