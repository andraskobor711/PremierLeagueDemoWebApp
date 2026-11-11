using Mapster;
using PremierLeagueWebApi.API.Schemas.Season;
using PremierLeagueWebApi.Core.Models;

namespace PremierLeagueWebApi.API.Mapping;

public class SeasonMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TeamsOfSeasonSchema, RawSeasonTeamsUpdate>()
            .Map(dest => dest.TeamIds, src => src.Teams.Select(t => t.Id));
    }
}
