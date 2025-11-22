using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessorsController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public ProfessorsController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,professor")]
    public async Task<ActionResult<ProfessorDto>> GetProfessor(Guid id)
    {
        try
        {
            var response = await _supabase.From<Professor>()
                .Where(p => p.Id == id)
                .Single();
            
            if (response == null)
                return NotFound();

            var facultyName = "";
            if (response.FacultyId.HasValue)
            {
                var facultyResponse = await _supabase.From<Faculty>()
                    .Where(f => f.Id == response.FacultyId.Value)
                    .Single();
                facultyName = facultyResponse?.Name ?? "";
            }

            var professorDto = new ProfessorDto
            {
                Id = response.Id,
                FacultyId = response.FacultyId,
                FacultyName = facultyName
            };

            return Ok(professorDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving professor details: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ProfessorDto>> CreateOrUpdateProfessor([FromBody] CreateProfessorRequest request)
    {
        try
        {
            var professor = new Professor
            {
                Id = request.Id,
                FacultyId = request.FacultyId
            };

            // Upsert logic
            var response = await _supabase.From<Professor>()
                .Upsert(professor);
            
            var created = response.Models.FirstOrDefault();
            if (created == null)
                return BadRequest("Failed to save professor details");

            var professorDto = new ProfessorDto
            {
                Id = created.Id,
                FacultyId = created.FacultyId
            };

            return Ok(professorDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error saving professor details: {ex.Message}");
        }
    }
}

public class ProfessorDto
{
    public Guid Id { get; set; }
    public long? FacultyId { get; set; }
    public string? FacultyName { get; set; }
}

public class CreateProfessorRequest
{
    public Guid Id { get; set; }
    public long FacultyId { get; set; }
}
