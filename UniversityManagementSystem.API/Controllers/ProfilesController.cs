using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin,rector")]
public class ProfilesController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public ProfilesController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfiles()
    {
        var response = await _supabase.From<Profile>().Get();
        return Ok(response.Models);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        var response = await _supabase.From<Profile>().Where(p => p.Id == id).Get();
        var profile = response.Models.FirstOrDefault();

        if (profile == null)
        {
            return NotFound();
        }

        return Ok(profile);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] Profile profile)
    {
        var existingResponse = await _supabase.From<Profile>().Where(p => p.Id == id).Get();
        if (!existingResponse.Models.Any())
        {
            return NotFound();
        }

        profile.Id = id;
        var response = await _supabase.From<Profile>().Update(profile);
        return Ok(response.Models.FirstOrDefault());
    }

    [HttpPut("{id}/student")]
    public async Task<IActionResult> UpdateStudentInfo(Guid id, [FromBody] Student student)
    {
        student.Id = id;
        
        // Check if student record exists
        var existingResponse = await _supabase.From<Student>().Where(s => s.Id == id).Get();
        
        if (existingResponse.Models.Any())
        {
            var response = await _supabase.From<Student>().Update(student);
            return Ok(response.Models.FirstOrDefault());
        }
        else
        {
            var response = await _supabase.From<Student>().Insert(student);
            return Ok(response.Models.FirstOrDefault());
        }
    }

    [HttpPut("{id}/professor")]
    public async Task<IActionResult> UpdateProfessorInfo(Guid id, [FromBody] Professor professor)
    {
        professor.Id = id;
        
        // Check if professor record exists
        var existingResponse = await _supabase.From<Professor>().Where(p => p.Id == id).Get();
        
        if (existingResponse.Models.Any())
        {
            var response = await _supabase.From<Professor>().Update(professor);
            return Ok(response.Models.FirstOrDefault());
        }
        else
        {
            var response = await _supabase.From<Professor>().Insert(professor);
            return Ok(response.Models.FirstOrDefault());
        }
    }
}
