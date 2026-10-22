namespace PremierLeagueWebApi.API.Schemas.Match
{
    public record MatchSchema
    {
        public int Id { get; init; }

        public required int SeasonId { get; set; }

        public required string SeasonLabel { get; init; }

        public required int HomeTeamId { get; set; }

        public required string HomeTeamName { get; init; }

        public string? HomeTeamLogoSrc { get; set; }

        public required int AwayTeamId { get; set; }

        public required string AwayTeamName { get; init; }

        public string? AwayTeamLogoSrc { get; set; }

        public required int? GameWeekNumber { get; init; }

        public required DateTime KickoffAt { get; init; }

        public int? HomeScore { get; init; }

        public int? AwayScore { get; init; }
    }
}
