using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public GroupsController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups([FromQuery] long? programId = null)
    {
        var query = _supabase.From<Group>();
        
        if (programId.HasValue)
        {
            var response = await query.Where(g => g.ProgramId == programId.Value).Get();
            return Ok(response.Models);
        }
        
        var allResponse = await query.Get();
        return Ok(allResponse.Models);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroup(long id)
    {
        var response = await _supabase.From<Group>().Where(g => g.Id == id).Get();
        var group = response.Models.FirstOrDefault();

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }

    [HttpPost]
    [Authorize(Roles = "admin,rector,dean")]
    public async Task<IActionResult> CreateGroup([FromBody] Group group)
    {
        if (string.IsNullOrWhiteSpace(group.Name))
        {
            return BadRequest("Name is required.");
        }

        var response = await _supabase.From<Group>().Insert(group);
        var newGroup = response.Models.FirstOrDefault();

        return CreatedAtAction(nameof(GetGroup), new { id = newGroup?.Id }, newGroup);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,rector,dean")]
    public async Task<IActionResult> UpdateGroup(long id, [FromBody] Group group)
    {
        var existingResponse = await _supabase.From<Group>().Where(g => g.Id == id).Get();
        if (!existingResponse.Models.Any())
        {
            return NotFound();
        }

        group.Id = id;
        var response = await _supabase.From<Group>().Update(group);
        return Ok(response.Models.FirstOrDefault());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,rector,dean")]
    public async Task<IActionResult> DeleteGroup(long id)
    {
        await _supabase.From<Group>().Where(g => g.Id == id).Delete();
        return NoContent();
    }
}
