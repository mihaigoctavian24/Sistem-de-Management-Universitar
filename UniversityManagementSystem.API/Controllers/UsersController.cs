using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public UsersController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<List<ProfileDto>>> GetUsers()
    {
        try
        {
            var response = await _supabase.From<Profile>()
                .Get();
            
            var profiles = response.Models;
            
            var profileDtos = profiles.Select(p => new ProfileDto
            {
                Id = p.Id,
                FirstName = p.FirstName ?? "",
                LastName = p.LastName ?? "",
                Email = p.Email ?? "",
                Role = p.Role
            }).ToList();

            return Ok(profileDtos);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving users: {ex.Message}");
        }
    }

    [HttpPut("{id}/role")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> UpdateUserRole(Guid id, [FromBody] UpdateRoleRequest request)
    {
        try
        {
            var profile = new Profile
            {
                Id = id,
                Role = request.Role
            };

            var response = await _supabase.From<Profile>()
                .Where(p => p.Id == id)
                .Update(profile);
            
            var updated = response.Models.FirstOrDefault();
            if (updated == null)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating user role: {ex.Message}");
        }
    }
}

public class UpdateRoleRequest
{
    public string Role { get; set; } = string.Empty;
}
