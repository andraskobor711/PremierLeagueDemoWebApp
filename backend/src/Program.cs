using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PremierLeagueWebApi.Core.Managers;
using PremierLeagueWebApi.Core.Repositories;
using PremierLeagueWebApi.Core.Security;
using PremierLeagueWebApi.Core.Services;
using PremierLeagueWebApi.Infrastructure.Auth;
using PremierLeagueWebApi.Infrastructure;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

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

builder.Services.AddIdentityCore<IdentityUser>(options =>
    {
        options.Password.RequiredLength = 12;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PlContext>()
    .AddSignInManager();

var authOptions = builder.Configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>();
if (authOptions is null || !AuthOptions.IsValid(authOptions))
{
    throw new InvalidOperationException(AuthOptions.ValidationError);
}

builder.Services.AddSingleton(Options.Create(authOptions));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = authOptions.Jwt.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Jwt.SigningKey)),
            NameClaimType = JwtRegisteredClaimNames.Sub,
            RoleClaimType = "role"
        };
    });
builder.Services.AddAuthorization(options =>
{
    foreach (var claim in AuthorizationClaims.All)
    {
        options.AddPolicy(claim, policy =>
            policy.RequireAuthenticatedUser().RequireClaim(claim, "true"));
    }
});

AddMappings(builder.Services);

// BLL Repositories
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ISeasonRepository, SeasonRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();

// BLL Managers
builder.Services.AddScoped<IMatchManager, MatchManager>();
builder.Services.AddScoped<ISeasonManager, SeasonManager>();
builder.Services.AddScoped<ITableManager, TableManager>();

// BLL Servies
builder.Services.AddScoped<ITableService, TableService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();

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