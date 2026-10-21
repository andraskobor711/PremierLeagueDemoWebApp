using Mapster;
using Microsoft.EntityFrameworkCore;
using PremierLeagueWebApi.Core.Managers;
using PremierLeagueWebApi.Core.Repositories;
using PremierLeagueWebApi.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddDbContext<PlContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DemoSqliteConnection")));

AddMappings(builder.Services);

// BLL Repositories
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ISeasonRepository, SeasonRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();

// BLL Managers
builder.Services.AddScoped<IMatchManager, MatchManager>();
builder.Services.AddScoped<ISeasonManager, SeasonManager>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

static void AddMappings(IServiceCollection services)
{
    var config = TypeAdapterConfig.GlobalSettings;
    config.Scan(Assembly.GetExecutingAssembly());
    services.AddSingleton(config);
}