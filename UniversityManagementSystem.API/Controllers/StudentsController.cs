using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "secretary,admin,professor,dean,rector")]
public class StudentsController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public StudentsController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var response = await _supabase.From<Student>().Get();
        return Ok(response.Models);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudent(Guid id)
    {
        var response = await _supabase.From<Student>()
            .Where(s => s.Id == id)
            .Get();
        var student = response.Models.FirstOrDefault();

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpGet("group/{groupId}")]
    public async Task<IActionResult> GetStudentsByGroup(long groupId)
    {
        var response = await _supabase.From<Student>()
            .Where(s => s.GroupId == groupId)
            .Get();
        return Ok(response.Models);
    }

    [HttpPost]
    [Authorize(Roles = "secretary,admin")]
    public async Task<IActionResult> CreateStudent([FromBody] Student student)
    {
        var response = await _supabase.From<Student>().Insert(student);
        var created = response.Models.FirstOrDefault();
        return CreatedAtAction(nameof(GetStudent), new { id = created?.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "secretary,admin")]
    public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] Student student)
    {
        var existingResponse = await _supabase.From<Student>()
            .Where(s => s.Id == id)
            .Get();
        
        if (!existingResponse.Models.Any())
        {
            return NotFound();
        }

        student.Id = id;
        var response = await _supabase.From<Student>().Update(student);
        return Ok(response.Models.FirstOrDefault());
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "secretary,admin")]
    public async Task<IActionResult> UpdateStudentStatus(Guid id, [FromBody] StudentStatusUpdate statusUpdate)
    {
        var existingResponse = await _supabase.From<Student>()
            .Where(s => s.Id == id)
            .Get();
        
        var student = existingResponse.Models.FirstOrDefault();
        if (student == null)
        {
            return NotFound();
        }

        student.Status = statusUpdate.Status;
        var response = await _supabase.From<Student>().Update(student);
        return Ok(response.Models.FirstOrDefault());
    }
}

public class StudentStatusUpdate
{
    public string Status { get; set; } = string.Empty;
}
