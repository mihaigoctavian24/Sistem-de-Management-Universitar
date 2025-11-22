using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public StudentsController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,student")]
    public async Task<ActionResult<StudentDto>> GetStudent(Guid id)
    {
        try
        {
            var response = await _supabase.From<Student>()
                .Where(s => s.Id == id)
                .Single();
            
            if (response == null)
                return NotFound();

            var groupName = "";
            if (response.GroupId.HasValue)
            {
                var groupResponse = await _supabase.From<Group>()
                    .Where(g => g.Id == response.GroupId.Value)
                    .Single();
                groupName = groupResponse?.Name ?? "";
            }

            var studentDto = new StudentDto
            {
                Id = response.Id,
                GroupId = response.GroupId,
                GroupName = groupName,
                RegistrationNumber = response.RegistrationNumber,
                Status = response.Status,
                EnrollmentDate = response.EnrollmentDate
            };

            return Ok(studentDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving student details: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<StudentDto>> CreateOrUpdateStudent([FromBody] CreateStudentRequest request)
    {
        try
        {
            var student = new Student
            {
                Id = request.Id,
                GroupId = request.GroupId,
                RegistrationNumber = request.RegistrationNumber,
                Status = "active",
                EnrollmentDate = DateTime.UtcNow
            };

            // Upsert logic (Insert or Update)
            var response = await _supabase.From<Student>()
                .Upsert(student);
            
            var created = response.Models.FirstOrDefault();
            if (created == null)
                return BadRequest("Failed to save student details");

            var studentDto = new StudentDto
            {
                Id = created.Id,
                GroupId = created.GroupId,
                RegistrationNumber = created.RegistrationNumber,
                Status = created.Status,
                EnrollmentDate = created.EnrollmentDate
            };

            return Ok(studentDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error saving student details: {ex.Message}");
        }
    }
}

public class StudentDto
{
    public Guid Id { get; set; }
    public long? GroupId { get; set; }
    public string? GroupName { get; set; }
    public string? RegistrationNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? EnrollmentDate { get; set; }
}

public class CreateStudentRequest
{
    public Guid Id { get; set; }
    public long GroupId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
}
