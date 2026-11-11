using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueWebApi.API.Schemas.Auth;
using PremierLeagueWebApi.Core.Security;

namespace PremierLeagueWebApi.API.Controllers;

[ApiController]
[Route("auth/roles/{roleName}/claims")]
[Authorize(Roles = UserRoles.Admin)]
public class RoleClaimsController(RoleManager<IdentityRole> _roleManager) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<RoleClaimSchema>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<RoleClaimSchema>>> GetRoleClaims(
        [FromRoute] string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role is null)
        {
            return NotFound();
        }

        var claims = await _roleManager.GetClaimsAsync(role);
        return Ok(claims
            .Select(claim => new RoleClaimSchema { Type = claim.Type, Value = claim.Value })
            .OrderBy(claim => claim.Type)
            .ThenBy(claim => claim.Value)
            .ToList());
    }

    [HttpPost]
    [ProducesResponseType(typeof(RoleClaimSchema), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RoleClaimSchema>> AddRoleClaim(
        [FromRoute] string roleName,
        [FromBody] RoleClaimSchema roleClaim)
    {
        if (!AuthorizationClaims.All.Contains(roleClaim.Type, StringComparer.Ordinal)
            || roleClaim.Value != "true")
        {
            return BadRequest("Only supported permission claims with value 'true' can be added.");
        }

        var role = await _roleManager.FindByNameAsync(roleName);
        if (role is null)
        {
            return NotFound();
        }

        var claim = new Claim(roleClaim.Type, roleClaim.Value);
        var existingClaims = await _roleManager.GetClaimsAsync(role);
        if (existingClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
        {
            return Conflict("The role already has this claim.");
        }

        var result = await _roleManager.AddClaimAsync(role, claim);
        if (!result.Succeeded)
        {
            return Problem(
                detail: string.Join("; ", result.Errors.Select(error => error.Description)),
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return Ok(roleClaim);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveRoleClaim(
        [FromRoute] string roleName,
        [FromQuery] string type,
        [FromQuery] string value)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role is null)
        {
            return NotFound();
        }

        var claim = new Claim(type, value);
        var existingClaims = await _roleManager.GetClaimsAsync(role);
        List<Claim> matchingClaims = [.. existingClaims
            .Where(c => c.Type == claim.Type && c.Value == claim.Value)];
        if (matchingClaims.Count == 0)
        {
            return NotFound();
        }

        List<string> errors = [];
        foreach (var claimToRemove in matchingClaims)
        {
            var result = await _roleManager.RemoveClaimAsync(role, claimToRemove);
            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(err => err.Description));
            }
        }

        if (errors.Count != 0)
        {
            return Problem(
                detail: string.Join("; ", errors),
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}
