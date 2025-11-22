using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public GradesController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet("my")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> GetMyGrades()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var response = await _supabase.From<Grade>()
            .Select("*")
            .Filter("student_id", Supabase.Postgrest.Constants.Operator.Equals, userId)
            .Get();

        return Ok(response.Models);
    }

    [HttpGet("course/{courseId}")]
    [Authorize(Roles = "admin,professor,dean,rector")]
    public async Task<IActionResult> GetGradesByCourse(long courseId)
    {
        var response = await _supabase.From<Grade>()
            .Select("*")
            .Filter("course_id", Supabase.Postgrest.Constants.Operator.Equals, courseId)
            .Get();

        return Ok(response.Models);
    }

    [HttpPost]
    [Authorize(Roles = "admin,professor")]
    public async Task<IActionResult> CreateGrade([FromBody] Grade grade)
    {
        var response = await _supabase.From<Grade>().Insert(grade);
        return Ok(response.Models.FirstOrDefault());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,professor")]
    public async Task<IActionResult> DeleteGrade(long id)
    {
        await _supabase.From<Grade>().Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id).Delete();
        return NoContent();
    }
}
