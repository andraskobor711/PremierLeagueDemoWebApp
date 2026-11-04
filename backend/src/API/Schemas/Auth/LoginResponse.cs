namespace PremierLeagueWebApi.API.Schemas.Auth;

public sealed record LoginResponse(string AccessToken, string TokenType, DateTime ExpiresAtUtc);
