using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremierLeagueWebApi.Models;

public class Match
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int SeasonId { get; set; }
    public virtual Season Season { get; set; } = null!;

    public int HomeTeamId { get; set; }
    public virtual Team HomeTeam { get; set; } = null!;

    public int AwayTeamId { get; set; }
    public virtual Team AwayTeam { get; set; } = null!;

    public int? Matchweek { get; set; }

    public DateTimeOffset KickoffAt { get; set; }

    [Range(0, int.MaxValue)]
    public int? HomeScore { get; set; }

    [Range(0, int.MaxValue)]
    public int? AwayScore { get; set; }
}
