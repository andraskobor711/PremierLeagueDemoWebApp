using PremierLeagueWebApi.Core.Models;
using PremierLeagueWebApi.Core.Repositories;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public class MatchManager(IMatchRepository _matchRepository, ISeasonManager _seasonManager) : IMatchManager
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

    public async Task<IEnumerable<Match>> GetCurrentGameWeekMatches(CancellationToken? cancellationToken = null)
    {
        var gameWeek = await GetCurrentGameWeek();
        if (gameWeek is null)
        {
            return [];
        }

        return (await _matchRepository.GetByGameWeek(gameWeek.Season.Id, gameWeek.Number, cancellationToken))
            .OrderBy(m => m.KickoffAt).ThenBy(m => m.HomeTeam.UsedName);
    }

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

    public async Task<GameWeek> GetCurrentGameWeek(CancellationToken? cancellationToken = null)
    {
        var now = DateTime.UtcNow;
        var season = await _seasonManager.GetCurrentSeason()
            ?? throw new InvalidOperationException("Current season is not available.");
        var seasonId = season.Id;

        var matches = (await _matchRepository.GetBySeasonWithoutIncludes(seasonId))
            .Where(m => m.GameWeekNumber is not null)
            .OrderBy(m => m.KickoffAt);

        var gameWeekDeterminer = matches.SkipWhile(m => m.KickoffAt <= now).FirstOrDefault()
            ?? matches.LastOrDefault();

        var gameWeekNumber = gameWeekDeterminer is not null
            ? gameWeekDeterminer.GameWeekNumber!.Value   // null values filtered earlier
            : 1;

        return new GameWeek { Season = season, Number = gameWeekNumber };
    }
}
