using Mapster;
using PremierLeagueWebApi.API.Schemas.Table;
using PremierLeagueWebApi.Core.Models;

namespace PremierLeagueWebApi.API.Mapping;

public class TableMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Table, TableSchema>()
            .Map(dest => dest.SeasonId, src => src.SeasonBasicInfo.Id)
            .Map(dest => dest.SeasonLabel, src => src.SeasonBasicInfo.Label)
            .Map(dest => dest.Rows, src => src.SortedRows);

        config.NewConfig<TableRow, TableRowSchema>()
            .Map(dest => dest.TeamName, src => src.Team.UsedName)
            .Map(dest => dest.LogoSrc, src => src.Team.LogoSrc);
    }
}
