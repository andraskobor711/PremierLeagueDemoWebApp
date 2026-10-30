using PremierLeagueWebApi.API.Schemas.Season;

namespace PremierLeagueWebApi.API.Schemas.Table;

public class TableSchema
{
    public required int SeasonId { get; set; }

    public required string SeasonLabel { get; set; }

    public ICollection<TableRowSchema> Rows { get; set; } = [];
}
