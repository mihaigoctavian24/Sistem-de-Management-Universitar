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
    public async Task<ActionResult<List<FacultyDto>>> GetFaculties()
    {
        try
        {
            var response = await _supabase.From<Faculty>()
                .Get();
            
            var faculties = response.Models;
            
            // Get programs count for each faculty
            var programsResponse = await _supabase.From<StudyProgram>()
                .Get();
            var programs = programsResponse.Models;
            
            // Get dean profiles
            var profilesResponse = await _supabase.From<Profile>()
                .Get();
            var profiles = profilesResponse.Models;

            var facultyDtos = faculties.Select(f => new FacultyDto
            {
                Id = f.Id,
                Name = f.Name,
                DeanId = f.DeanId,
                DeanName = f.DeanId.HasValue 
                    ? profiles.FirstOrDefault(p => p.Id == f.DeanId)?.FirstName + " " + profiles.FirstOrDefault(p => p.Id == f.DeanId)?.LastName
                    : null,
                ProgramsCount = programs.Count(p => p.FacultyId == f.Id)
            }).ToList();

            return Ok(facultyDtos);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving faculties: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FacultyDto>> GetFaculty(long id)
    {
        try
        {
            var response = await _supabase.From<Faculty>()
                .Where(f => f.Id == id)
                .Single();
            
            if (response == null)
                return NotFound();

            var programsResponse = await _supabase.From<StudyProgram>()
                .Where(p => p.FacultyId == id)
                .Get();

            Profile? dean = null;
            if (response.DeanId.HasValue)
            {
                var deanResponse = await _supabase.From<Profile>()
                    .Where(p => p.Id == response.DeanId.Value)
                    .Single();
                dean = deanResponse;
            }

            var facultyDto = new FacultyDto
            {
                Id = response.Id,
                Name = response.Name,
                DeanId = response.DeanId,
                DeanName = dean != null ? $"{dean.FirstName} {dean.LastName}" : null,
                ProgramsCount = programsResponse.Models.Count
            };

            return Ok(facultyDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving faculty: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<FacultyDto>> CreateFaculty([FromBody] CreateFacultyRequest request)
    {
        try
        {
            var faculty = new Faculty
            {
                Name = request.Name,
                DeanId = request.DeanId
            };

            var response = await _supabase.From<Faculty>()
                .Insert(faculty);
            
            var created = response.Models.FirstOrDefault();
            if (created == null)
                return BadRequest("Failed to create faculty");

            var facultyDto = new FacultyDto
            {
                Id = created.Id,
                Name = created.Name,
                DeanId = created.DeanId,
                ProgramsCount = 0
            };

            return CreatedAtAction(nameof(GetFaculty), new { id = created.Id }, facultyDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error creating faculty: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<FacultyDto>> UpdateFaculty(long id, [FromBody] CreateFacultyRequest request)
    {
        try
        {
            var faculty = new Faculty
            {
                Id = id,
                Name = request.Name,
                DeanId = request.DeanId
            };

            var response = await _supabase.From<Faculty>()
                .Where(f => f.Id == id)
                .Update(faculty);
            
            var updated = response.Models.FirstOrDefault();
            if (updated == null)
                return NotFound();

            var facultyDto = new FacultyDto
            {
                Id = updated.Id,
                Name = updated.Name,
                DeanId = updated.DeanId
            };

            return Ok(facultyDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating faculty: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> DeleteFaculty(long id)
    {
        try
        {
            // Check if faculty has programs
            var programsResponse = await _supabase.From<StudyProgram>()
                .Where(p => p.FacultyId == id)
                .Get();
            
            if (programsResponse.Models.Any())
            {
                return BadRequest("Cannot delete faculty with existing programs. Please delete or reassign programs first.");
            }

            await _supabase.From<Faculty>()
                .Where(f => f.Id == id)
                .Delete();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Error deleting faculty: {ex.Message}");
        }
    }
}
