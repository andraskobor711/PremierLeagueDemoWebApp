namespace PremierLeagueWebApi.Core.Models;

public class Table
{
    public required SeasonBasicInfo SeasonBasicInfo { get; set; }

    public ICollection<TableRowWithPosition> SortedRows { get; set; } = [];
}
