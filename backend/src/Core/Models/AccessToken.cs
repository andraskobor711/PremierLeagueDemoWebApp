namespace PremierLeagueWebApi.Core.Models;

public sealed record AccessToken(string Value, DateTime ExpiresAtUtc);
