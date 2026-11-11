using Mapster;
using Microsoft.EntityFrameworkCore;
using PremierLeagueWebApi.Core.Exceptions;
using PremierLeagueWebApi.Core.Models;
using PremierLeagueWebApi.Infrastructure;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Repositories;

public class SeasonRepository(PlContext _plContext) : ISeasonRepository
{
    public async Task<IEnumerable<SeasonBasicInfo>> GetAllSeasonBasicInfos(CancellationToken? cancellationToken = default)
        => [.. await _plContext
            .Seasons
            .Select(s => new SeasonBasicInfo{Id = s.Id, Label = s.Label})
            .ToListAsync(cancellationToken ?? CancellationToken.None)];



    public async Task<SeasonBasicInfo?> GetSeasonBasicInfoById(int id, CancellationToken? cancellationToken = default)
        => (await _plContext
            .Seasons
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken ?? CancellationToken.None))
            .Adapt<SeasonBasicInfo>();

    public async Task<Season?> GetSeasonById(int id, CancellationToken? cancellationToken = default)
        => await _plContext
            .Seasons
            .Include(s => s.Teams)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken ?? CancellationToken.None);

    public async Task<ICollection<Team>?> GetTeamsOfSeason(
        int seasonId,
        CancellationToken? cancellationToken = default)
        => (await _plContext.Seasons
            .Include(s => s.Teams)
            .FirstOrDefaultAsync(
                s => s.Id == seasonId,
                cancellationToken ?? CancellationToken.None))
            ?.Teams;

    public async Task<SeasonBasicInfo?> GetSeasonBasicInfoByLabel(string label, CancellationToken? cancellationToken = default)
        => (await _plContext.Seasons
            .FirstOrDefaultAsync(s => s.Label == label, cancellationToken ?? CancellationToken.None))
            .Adapt<SeasonBasicInfo>();

    public async Task<int> AddSeason(Season season, CancellationToken? cancellationToken = default)
    {
        _plContext.Seasons.Add(season);
        await _plContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

        return season.Id;
    }

    public async Task<Season?> UpdateSeasonTeams(
        int id,
        SeasonTeamsUpdate seasonUpdate,
        CancellationToken? cancellationToken = default)
    {
        var storedSeason = await _plContext.Seasons
            .Include(s => s.Teams)
            .FirstOrDefaultAsync(s => s.Id == id);

        if(storedSeason is null)
        {
            return null;
        }

        storedSeason.Teams = [.. seasonUpdate.Teams];

        await _plContext.SaveChangesAsync();

        return storedSeason;
    }
}
