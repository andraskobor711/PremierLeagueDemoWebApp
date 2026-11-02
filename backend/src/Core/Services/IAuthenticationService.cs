using PremierLeagueWebApi.Core.Models;

namespace PremierLeagueWebApi.Core.Services;

public interface IAuthenticationService
{
    Task<AccessToken?> AuthenticateAsync(string username, string password);
}
