using Mapster;
using PremierLeagueWebApi.API.Schemas.Match;
using PremierLeagueWebApi.BLL.Dto;
using PremierLeagueWebApi.Models;

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

            config.NewConfig<(int Id, UpdateMatchSchema UpdateSchema), UpdateMatchDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.SeasonLabel, src => src.UpdateSchema.SeasonLabel)
                .Map(dest => dest.HomeTeamName, src => src.UpdateSchema.HomeTeamName)
                .Map(dest => dest.AwayTeamName, src => src.UpdateSchema.AwayTeamName)
                .Map(dest => dest.Matchweek, src => src.UpdateSchema.Matchweek)
                .Map(dest => dest.KickoffAt, src => src.UpdateSchema.KickoffAt)
                .Map(dest => dest.HomeScore, src => src.UpdateSchema.HomeScore)
                .Map(dest => dest.AwayScore, src => src.UpdateSchema.AwayScore);
        }
    }
}
