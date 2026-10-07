using Microsoft.EntityFrameworkCore;
using PremierLeagueWebApi.DAL;
using PremierLeagueWebApi.Models;

namespace PremierLeagueWebApi.BLL.Repositories;

public class TeamRepository(PlContext _plContext) : ITeamRepository
{
    public async Task<IEnumerable<Team>> GetAll(CancellationToken? cancellationToken)
        => await _plContext
            .Teams
            .OrderBy(t => t.FullName)
            .ToListAsync(cancellationToken ?? CancellationToken.None);

    public async Task<Team?> GetTeamById(int id, CancellationToken? cancellationToken)
        => await _plContext
            .Teams
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken ?? CancellationToken.None);

    public async Task<Team?> GetTeamByName(string name, CancellationToken? cancellationToken)
        => await _plContext
            .Teams
            .FirstOrDefaultAsync(t => t.UsedName == name, cancellationToken ?? CancellationToken.None);

    public async Task<int> Add(Team team, CancellationToken? cancellationToken)
    {
        _plContext.Teams.Add(team);
        await _plContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

        return team.Id;
    }

    public async Task<Team?> Update(Team team, CancellationToken? cancellationToken)
    {
        var existingTeam = await _plContext.Teams.FindAsync(
            [team.Id],
            cancellationToken ?? CancellationToken.None);

        if (existingTeam is null)
        {
            return null;
        }

        existingTeam.FullName = team.FullName;
        existingTeam.UsedName = team.UsedName;
        existingTeam.Abbreviation = team.Abbreviation;
        existingTeam.OfficialWebsite = team.OfficialWebsite;

        await _plContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

        return existingTeam;
    }

    public async Task<bool> Delete(int id, CancellationToken? cancellationToken)
    {
        var teamToDelete = await _plContext
            .Teams
            .FirstOrDefaultAsync(t => t.Id == id);

        if (teamToDelete is null)
        {
            return false;
        }

        _plContext.Teams.Remove(teamToDelete);
        await _plContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);

        return true;
    }
}
