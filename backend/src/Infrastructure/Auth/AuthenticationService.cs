using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PremierLeagueWebApi.Core.Models;
using PremierLeagueWebApi.Core.Services;

namespace PremierLeagueWebApi.Infrastructure.Auth;

public sealed class AuthenticationService : IAuthenticationService
{
    private const string RoleClaim = "role";
    private const string UnknownUsername = "unknown-user";

    private readonly AuthOptions _options;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly PasswordHasher<IdentityUser> _passwordHasher = new();
    private readonly IdentityUser _unknownUser = new() { UserName = UnknownUsername };
    private readonly string _dummyPasswordHash;

    public AuthenticationService(
        IOptions<AuthOptions> options,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _options = options.Value;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _dummyPasswordHash = _passwordHasher.HashPassword(
            _unknownUser,
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)));
    }

    public async Task<AccessToken?> AuthenticateAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
        {
            return null;
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            _passwordHasher.VerifyHashedPassword(_unknownUser, _dummyPasswordHash, password);
            return null;
        }

        var passwordCheck = await _signInManager.CheckPasswordSignInAsync(
            user,
            password,
            lockoutOnFailure: true);
        if (!passwordCheck.Succeeded)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        foreach (var roleName in roles)
        {
            claims.Add(new Claim(RoleClaim, roleName));
            var role = await _roleManager.FindByNameAsync(roleName)
                ?? throw new InvalidOperationException($"Role '{roleName}' assigned to a user does not exist.");
            claims.AddRange((await _roleManager.GetClaimsAsync(role))
                .Where(claim => claim.Type != RoleClaim));
        }

        claims.AddRange((await _userManager.GetClaimsAsync(user))
            .Where(claim => claim.Type != RoleClaim));

        var now = DateTime.UtcNow;
        var expiresAtUtc = now.AddMinutes(_options.Jwt.ExpirationMinutes);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Jwt.SigningKey)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _options.Jwt.Issuer,
            audience: _options.Jwt.Audience,
            claims: claims,
            notBefore: now,
            expires: expiresAtUtc,
            signingCredentials: signingCredentials);

        return new AccessToken(new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
    }
}
