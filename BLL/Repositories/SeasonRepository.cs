using Microsoft.EntityFrameworkCore;
using PremierLeagueWebApi.DAL;
using PremierLeagueWebApi.Models;

namespace PremierLeagueWebApi.BLL.Repositories;

public class SeasonRepository(PlContext _plContext) : ISeasonRepository
{
    public async Task<IEnumerable<Season>> GetAll(CancellationToken? cancellationToken = default)
        => [.. await _plContext
            .Seasons
            .ToListAsync(cancellationToken ?? CancellationToken.None)];

    public async Task<Season?> GetSeasonById(int id, CancellationToken? cancellationToken = default)
        => await _plContext
            .Seasons
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken ?? CancellationToken.None);

    public async Task<Season?> GetSeasonByLabel(string label, CancellationToken? cancellationToken = default)
        => await _plContext
            .Seasons
            .FirstOrDefaultAsync(s => s.Label == label, cancellationToken ?? CancellationToken.None);

    public async Task<int> Add(Season season, CancellationToken? cancellationToken = default)
    {
        _plContext.Seasons.Add(season);
        await _plContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

        return season.Id;
    }
}
