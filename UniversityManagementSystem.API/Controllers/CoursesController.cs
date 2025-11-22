using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public CoursesController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses([FromQuery] long? programId = null, [FromQuery] Guid? professorId = null)
    {
        var query = _supabase.From<Course>();
        
        if (programId.HasValue && professorId.HasValue)
        {
            var response = await query
                .Where(c => c.ProgramId == programId.Value && c.ProfessorId == professorId.Value)
                .Get();
            return Ok(response.Models);
        }
        else if (programId.HasValue)
        {
            var response = await query.Where(c => c.ProgramId == programId.Value).Get();
            return Ok(response.Models);
        }
        else if (professorId.HasValue)
        {
            var response = await query.Where(c => c.ProfessorId == professorId.Value).Get();
            return Ok(response.Models);
        }
        
        var allResponse = await query.Get();
        return Ok(allResponse.Models);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourse(long id)
    {
        var response = await _supabase.From<Course>().Where(c => c.Id == id).Get();
        var course = response.Models.FirstOrDefault();

        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpPost]
    [Authorize(Roles = "admin,rector,dean")]
    public async Task<IActionResult> CreateCourse([FromBody] Course course)
    {
        if (string.IsNullOrWhiteSpace(course.Name))
        {
            return BadRequest("Name is required.");
        }

        var response = await _supabase.From<Course>().Insert(course);
        var newCourse = response.Models.FirstOrDefault();

        return CreatedAtAction(nameof(GetCourse), new { id = newCourse?.Id }, newCourse);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,rector,dean")]
    public async Task<IActionResult> UpdateCourse(long id, [FromBody] Course course)
    {
        var existingResponse = await _supabase.From<Course>().Where(c => c.Id == id).Get();
        if (!existingResponse.Models.Any())
        {
            return NotFound();
        }

        course.Id = id;
        var response = await _supabase.From<Course>().Update(course);
        return Ok(response.Models.FirstOrDefault());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,rector,dean")]
    public async Task<IActionResult> DeleteCourse(long id)
    {
        await _supabase.From<Course>().Where(c => c.Id == id).Delete();
        return NoContent();
    }
}
