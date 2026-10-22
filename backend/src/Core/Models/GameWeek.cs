using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Models;

public class GameWeek
{
    public required Season Season { get; set; }

    public int Number { get; set; }
}
