using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgramsController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public ProgramsController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProgramDto>>> GetPrograms()
    {
        try
        {
            var response = await _supabase.From<StudyProgram>()
                .Get();
            
            var programs = response.Models;
            
            // Get faculties
            var facultiesResponse = await _supabase.From<Faculty>()
                .Get();
            var faculties = facultiesResponse.Models;
            
            // Get groups count
            var groupsResponse = await _supabase.From<Group>()
                .Get();
            var groups = groupsResponse.Models;

            var programDtos = programs.Select(p => new ProgramDto
            {
                Id = p.Id,
                Name = p.Name,
                FacultyId = p.FacultyId,
                FacultyName = p.FacultyId.HasValue 
                    ? faculties.FirstOrDefault(f => f.Id == p.FacultyId)?.Name 
                    : null,
                StudyType = p.StudyType,
                Duration = p.Duration,
                GroupsCount = groups.Count(g => g.ProgramId == p.Id)
            }).ToList();

            return Ok(programDtos);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving programs: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProgramDto>> GetProgram(long id)
    {
        try
        {
            var response = await _supabase.From<StudyProgram>()
                .Where(p => p.Id == id)
                .Single();
            
            if (response == null)
                return NotFound();

            var facultyResponse = await _supabase.From<Faculty>()
                .Where(f => f.Id == response.FacultyId)
                .Single();
            
            var groupsResponse = await _supabase.From<Group>()
                .Where(g => g.ProgramId == id)
                .Get();

            var programDto = new ProgramDto
            {
                Id = response.Id,
                Name = response.Name,
                FacultyId = response.FacultyId,
                FacultyName = facultyResponse?.Name,
                StudyType = response.StudyType,
                Duration = response.Duration,
                GroupsCount = groupsResponse.Models.Count
            };

            return Ok(programDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving program: {ex.Message}");
        }
    }

    [HttpGet("faculty/{facultyId}")]
    public async Task<ActionResult<List<ProgramDto>>> GetProgramsByFaculty(long facultyId)
    {
        try
        {
            var response = await _supabase.From<StudyProgram>()
                .Where(p => p.FacultyId == facultyId)
                .Get();
            
            var programs = response.Models;
            
            var facultyResponse = await _supabase.From<Faculty>()
                .Where(f => f.Id == facultyId)
                .Single();

            var groupsResponse = await _supabase.From<Group>()
                .Get();
            var groups = groupsResponse.Models;

            var programDtos = programs.Select(p => new ProgramDto
            {
                Id = p.Id,
                Name = p.Name,
                FacultyId = p.FacultyId,
                FacultyName = facultyResponse?.Name,
                StudyType = p.StudyType,
                Duration = p.Duration,
                GroupsCount = groups.Count(g => g.ProgramId == p.Id)
            }).ToList();

            return Ok(programDtos);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving programs by faculty: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ProgramDto>> CreateProgram([FromBody] CreateProgramRequest request)
    {
        try
        {
            var program = new StudyProgram
            {
                Name = request.Name,
                FacultyId = request.FacultyId,
                StudyType = request.StudyType,
                Duration = request.Duration
            };

            var response = await _supabase.From<StudyProgram>()
                .Insert(program);
            
            var created = response.Models.FirstOrDefault();
            if (created == null)
                return BadRequest("Failed to create program");

            var programDto = new ProgramDto
            {
                Id = created.Id,
                Name = created.Name,
                FacultyId = created.FacultyId,
                StudyType = created.StudyType,
                Duration = created.Duration,
                GroupsCount = 0
            };

            return CreatedAtAction(nameof(GetProgram), new { id = created.Id }, programDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error creating program: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ProgramDto>> UpdateProgram(long id, [FromBody] CreateProgramRequest request)
    {
        try
        {
            var program = new StudyProgram
            {
                Id = id,
                Name = request.Name,
                FacultyId = request.FacultyId,
                StudyType = request.StudyType,
                Duration = request.Duration
            };

            var response = await _supabase.From<StudyProgram>()
                .Where(p => p.Id == id)
                .Update(program);
            
            var updated = response.Models.FirstOrDefault();
            if (updated == null)
                return NotFound();

            var programDto = new ProgramDto
            {
                Id = updated.Id,
                Name = updated.Name,
                FacultyId = updated.FacultyId,
                StudyType = updated.StudyType,
                Duration = updated.Duration
            };

            return Ok(programDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating program: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> DeleteProgram(long id)
    {
        try
        {
            // Check if program has groups
            var groupsResponse = await _supabase.From<Group>()
                .Where(g => g.ProgramId == id)
                .Get();
            
            if (groupsResponse.Models.Any())
            {
                return BadRequest("Cannot delete program with existing groups. Please delete or reassign groups first.");
            }

            await _supabase.From<StudyProgram>()
                .Where(p => p.Id == id)
                .Delete();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Error deleting program: {ex.Message}");
        }
    }
}
