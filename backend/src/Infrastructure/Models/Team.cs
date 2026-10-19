using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremierLeagueWebApi.Infrastructure.Models;

public class Team
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [DisplayName("Team name")]
    public required string FullName { get; set; }

    [Required]
    [MaxLength(100)]
    [DisplayName("Used name")]
    public required string UsedName { get; set; }

    [Required]
    [MaxLength(5)]
    [DisplayName("Name abbreviation")]
    public required string Abbreviation { get; set; }

    [MaxLength(50)]
    [DisplayName("Logo source")]
    public string? LogoSrc { get; set; }

    [MaxLength(100)]
    [DisplayName("Official website")]
    public string? OfficialWebsite { get; set; }

    public virtual ICollection<Season> Seasons { get; set; } = [];
}
