using PremierLeagueWebApi.Core.Dto;
using PremierLeagueWebApi.Core.Repositories;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Managers;

public class MatchManager(
    IMatchRepository _matchRepository,
    ITeamRepository _teamRepository,
    ISeasonRepository _seasonRepository) : IMatchManager
{
    public async Task<IEnumerable<Match>> GetAllMatches(CancellationToken? cancellationToken = null)
        => await _matchRepository.GetAll(cancellationToken);

    public async Task<Match?> GetMatchById(int id, CancellationToken? cancellationToken = null)
        => await _matchRepository.GetById(id, cancellationToken);

    public async Task<Match?> AddNewMatch(CreateMatchDto createMatchDto, CancellationToken? cancellationToken = null)
    {
        var homeTeam = await _teamRepository.GetTeamByName(createMatchDto.HomeTeamName, cancellationToken)
            ?? throw new InvalidOperationException($"Home team '{createMatchDto.HomeTeamName}' does not exist.");
        var awayTeam = await _teamRepository.GetTeamByName(createMatchDto.AwayTeamName, cancellationToken)
            ?? throw new InvalidOperationException($"Away team '{createMatchDto.AwayTeamName}' does not exist.");
        var season = await _seasonRepository.GetSeasonByLabel(createMatchDto.SeasonLabel, cancellationToken)
            ?? throw new InvalidOperationException($"Season '{createMatchDto.SeasonLabel}' does not exist.");
        // todo: more validation, e.g. a team cannot be involved for two games on the same Matchweek

        var match = new Match
        {
            HomeTeamId = homeTeam.Id,
            AwayTeamId = awayTeam.Id,
            SeasonId = season.Id,
            KickoffAt = createMatchDto.KickoffAt,
            Matchweek = createMatchDto.Matchweek,
            HomeScore = createMatchDto.HomeScore,
            AwayScore = createMatchDto.AwayScore,
        };

        await _matchRepository.Add(match, cancellationToken);

        var createdMatch = await _matchRepository.GetById(match.Id, cancellationToken);

        return createdMatch;
    }

    public async Task<Match?> UpdateMatch(UpdateMatchDto updateMatchDto, CancellationToken? cancellationToken = null)
    {
        var homeTeam = await _teamRepository.GetTeamByName(updateMatchDto.HomeTeamName, cancellationToken)
            ?? throw new InvalidOperationException($"Home team '{updateMatchDto.HomeTeamName}' does not exist.");
        var awayTeam = await _teamRepository.GetTeamByName(updateMatchDto.AwayTeamName, cancellationToken)
            ?? throw new InvalidOperationException($"Away team '{updateMatchDto.AwayTeamName}' does not exist.");
        var season = await _seasonRepository.GetSeasonByLabel(updateMatchDto.SeasonLabel, cancellationToken)
            ?? throw new InvalidOperationException($"Season '{updateMatchDto.SeasonLabel}' does not exist.");
        // todo: more validation, e.g. a team cannot be involved for two games on the same Matchweek

        var matchUpdate = new Match
        {
            Id = updateMatchDto.Id,
            HomeTeamId = homeTeam.Id,
            AwayTeamId = awayTeam.Id,
            SeasonId = season.Id,
            KickoffAt = updateMatchDto.KickoffAt,
            Matchweek = updateMatchDto.Matchweek,
            HomeScore = updateMatchDto.HomeScore,
            AwayScore = updateMatchDto.AwayScore,
        };

        await _matchRepository.Update(matchUpdate, cancellationToken);

        var updatedMatch = await _matchRepository.GetById(matchUpdate.Id, cancellationToken);

        return updatedMatch;
    }

    public async Task<bool> DeleteMatch(int id, CancellationToken? cancellationToken = null)
        => await _matchRepository.Delete(id, cancellationToken);
}
