using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "secretary,admin,rector")]
public class EnrollmentsController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public EnrollmentsController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet]
    public async Task<IActionResult> GetEnrollments()
    {
        var response = await _supabase.From<Enrollment>()
            .Order("created_at", Supabase.Postgrest.Constants.Ordering.Descending)
            .Get();
        return Ok(response.Models);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEnrollment(long id)
    {
        var response = await _supabase.From<Enrollment>()
            .Where(e => e.Id == id)
            .Get();
        var enrollment = response.Models.FirstOrDefault();

        if (enrollment == null)
        {
            return NotFound();
        }

        return Ok(enrollment);
    }

    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentEnrollments(Guid studentId)
    {
        var response = await _supabase.From<Enrollment>()
            .Where(e => e.StudentId == studentId)
            .Order("created_at", Supabase.Postgrest.Constants.Ordering.Descending)
            .Get();
        return Ok(response.Models);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetEnrollmentsByStatus(string status)
    {
        var response = await _supabase.From<Enrollment>()
            .Where(e => e.Status == status)
            .Order("created_at", Supabase.Postgrest.Constants.Ordering.Descending)
            .Get();
        return Ok(response.Models);
    }

    [HttpPost]
    [Authorize(Roles = "secretary,admin")]
    public async Task<IActionResult> CreateEnrollment([FromBody] Enrollment enrollment)
    {
        var response = await _supabase.From<Enrollment>().Insert(enrollment);
        var created = response.Models.FirstOrDefault();
        return CreatedAtAction(nameof(GetEnrollment), new { id = created?.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "secretary,admin")]
    public async Task<IActionResult> UpdateEnrollment(long id, [FromBody] Enrollment enrollment)
    {
        var existingResponse = await _supabase.From<Enrollment>()
            .Where(e => e.Id == id)
            .Get();
        
        if (!existingResponse.Models.Any())
        {
            return NotFound();
        }

        enrollment.Id = id;
        var response = await _supabase.From<Enrollment>().Update(enrollment);
        return Ok(response.Models.FirstOrDefault());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteEnrollment(long id)
    {
        var existingResponse = await _supabase.From<Enrollment>()
            .Where(e => e.Id == id)
            .Get();
        
        if (!existingResponse.Models.Any())
        {
            return NotFound();
        }

        await _supabase.From<Enrollment>()
            .Where(e => e.Id == id)
            .Delete();
        
        return NoContent();
    }
}
