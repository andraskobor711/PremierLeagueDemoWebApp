namespace PremierLeagueWebApi.API.Schemas;

public record ErrorSchema
{
    public required string ErrorType { get; init; }
    public required string Message { get; init; }
}
