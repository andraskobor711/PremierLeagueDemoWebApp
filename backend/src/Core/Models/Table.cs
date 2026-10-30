using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Core.Models;

public class Table
{
    public required Season Season { get; set; }

    public ICollection<TableRowWithPosition> SortedRows { get; set; } = [];
}
