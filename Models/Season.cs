using System.ComponentModel.DataAnnotations;

namespace PremierLeagueWebApi.Models;

public class Season
{
    public int Id { get; set; }

    [Required]
    [RegularExpression(@"^[1-2]\d{3}\/\d{2}$", ErrorMessage = "The season must match the following pattern: yyyy/yy")]
    public required string Label { get; set; }

    public virtual ICollection<Team> Teams { get; set; } = [];
}
