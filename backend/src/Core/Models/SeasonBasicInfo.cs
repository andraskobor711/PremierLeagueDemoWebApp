namespace PremierLeagueWebApi.Core.Models;

public record SeasonBasicInfo
{
    public required int Id { get; set; }

    public required string Label { get; set; }
}
