using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacultiesController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public FacultiesController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet]
    public async Task<IActionResult> GetFaculties()
    {
        var response = await _supabase.From<Faculty>().Get();
        return Ok(response.Models);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFaculty(long id)
    {
        var response = await _supabase.From<Faculty>().Where(f => f.Id == id).Get();
        var faculty = response.Models.FirstOrDefault();

        if (faculty == null)
        {
            return NotFound();
        }

        return Ok(faculty);
    }

    [HttpPost]
    [Authorize(Roles = "admin,rector")]
    public async Task<IActionResult> CreateFaculty([FromBody] Faculty faculty)
    {
        if (string.IsNullOrWhiteSpace(faculty.Name))
        {
            return BadRequest("Name is required.");
        }

        var response = await _supabase.From<Faculty>().Insert(faculty);
        var newFaculty = response.Models.FirstOrDefault();

        return CreatedAtAction(nameof(GetFaculty), new { id = newFaculty?.Id }, newFaculty);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,rector")]
    public async Task<IActionResult> UpdateFaculty(long id, [FromBody] Faculty faculty)
    {
        var existingResponse = await _supabase.From<Faculty>().Where(f => f.Id == id).Get();
        if (!existingResponse.Models.Any())
        {
            return NotFound();
        }

        faculty.Id = id; // Ensure ID matches
        var response = await _supabase.From<Faculty>().Update(faculty);
        return Ok(response.Models.FirstOrDefault());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,rector")]
    public async Task<IActionResult> DeleteFaculty(long id)
    {
        await _supabase.From<Faculty>().Where(f => f.Id == id).Delete();
        return NoContent();
    }
}
