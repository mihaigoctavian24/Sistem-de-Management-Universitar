using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public AttendanceController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet("my")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> GetMyAttendance()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var response = await _supabase.From<Attendance>()
            .Select("*")
            .Filter("student_id", Supabase.Postgrest.Constants.Operator.Equals, userId)
            .Get();

        return Ok(response.Models);
    }

    [HttpGet("course/{courseId}")]
    [Authorize(Roles = "admin,professor,dean,rector")]
    public async Task<IActionResult> GetAttendanceByCourse(long courseId, [FromQuery] DateTime? date)
    {
        var query = _supabase.From<Attendance>()
            .Select("*")
            .Filter("course_id", Supabase.Postgrest.Constants.Operator.Equals, courseId);

        if (date.HasValue)
        {
            query = query.Filter("date", Supabase.Postgrest.Constants.Operator.Equals, date.Value.ToString("yyyy-MM-dd"));
        }

        var response = await query.Get();
        return Ok(response.Models);
    }

    [HttpPost]
    [Authorize(Roles = "admin,professor")]
    public async Task<IActionResult> UpsertAttendance([FromBody] List<Attendance> attendanceRecords)
    {
        if (attendanceRecords == null || !attendanceRecords.Any())
            return BadRequest("No attendance records provided.");

        var response = await _supabase.From<Attendance>().Upsert(attendanceRecords);
        return Ok(response.Models);
    }
}
