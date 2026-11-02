using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PremierLeagueWebApi.Core.Security;

namespace PremierLeagueWebApi.Infrastructure.Auth;

public static class IdentityDevelopmentSeeder
{
    private static readonly (string Username, string Password, string Role)[] DemoUsers =
    [
        ("adminuser", "admin_p@$$word", UserRoles.Admin),
        //("demouser", "demo_p@$$word", UserRoles.User)
    ];

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        foreach (var roleName in new[] { UserRoles.Admin, UserRoles.User })
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                role = new IdentityRole(roleName);
                EnsureSucceeded(await roleManager.CreateAsync(role), $"creating role '{roleName}'");
            }

            foreach (var claimType in AuthorizationClaims.ForRole(roleName))
            {
                var existingClaims = await roleManager.GetClaimsAsync(role);
                if (!existingClaims.Any(claim => claim.Type == claimType && claim.Value == "true"))
                {
                    EnsureSucceeded(
                        await roleManager.AddClaimAsync(role, new Claim(claimType, "true")),
                        $"adding the '{claimType}' claim to role '{roleName}'");
                }
            }
        }

        foreach (var (username, password, roleName) in DemoUsers)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user is null)
            {
                user = new IdentityUser
                {
                    UserName = username,
                    LockoutEnabled = true
                };
                EnsureSucceeded(
                    await userManager.CreateAsync(user, password),
                    $"creating development user '{username}'");
            }

            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                EnsureSucceeded(
                    await userManager.AddToRoleAsync(user, roleName),
                    $"assigning development user '{username}' to role '{roleName}'");
            }
        }
    }

    private static void EnsureSucceeded(IdentityResult result, string operation)
    {
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(error => $"{error.Code}: {error.Description}"));
            throw new InvalidOperationException($"Failed while {operation}: {errors}");
        }
    }
}
