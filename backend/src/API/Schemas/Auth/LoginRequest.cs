using System.ComponentModel.DataAnnotations;

namespace PremierLeagueWebApi.API.Schemas.Auth;

public sealed class LoginRequest
{
    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}
