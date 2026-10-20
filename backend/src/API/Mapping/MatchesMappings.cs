using Mapster;
using PremierLeagueWebApi.API.Schemas.Match;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.API.Mapping
{
    public class MatchesMappings : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Match, MatchSchema>()
                .Map(dest => dest.HomeTeamName, src => src.HomeTeam.UsedName)
                .Map(dest => dest.AwayTeamName, src => src.AwayTeam.UsedName)
                .Map(dest => dest.SeasonLabel, src => src.Season.Label);
        }
    }
}
