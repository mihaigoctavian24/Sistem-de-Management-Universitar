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
    public async Task<IActionResult> GetPrograms([FromQuery] long? facultyId = null)
    {
        var query = _supabase.From<StudyProgram>();
        
        if (facultyId.HasValue)
        {
            var response = await query.Where(p => p.FacultyId == facultyId.Value).Get();
            return Ok(response.Models);
        }
        
        var allResponse = await query.Get();
        return Ok(allResponse.Models);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProgram(long id)
    {
        var response = await _supabase.From<StudyProgram>().Where(p => p.Id == id).Get();
        var program = response.Models.FirstOrDefault();

        if (program == null)
        {
            return NotFound();
        }

        return Ok(program);
    }

    [HttpPost]
    [Authorize(Roles = "admin,rector,dean")]
    public async Task<IActionResult> CreateProgram([FromBody] StudyProgram program)
    {
        if (string.IsNullOrWhiteSpace(program.Name))
        {
            return BadRequest("Name is required.");
        }

        var response = await _supabase.From<StudyProgram>().Insert(program);
        var newProgram = response.Models.FirstOrDefault();

        return CreatedAtAction(nameof(GetProgram), new { id = newProgram?.Id }, newProgram);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,rector,dean")]
    public async Task<IActionResult> UpdateProgram(long id, [FromBody] StudyProgram program)
    {
        var existingResponse = await _supabase.From<StudyProgram>().Where(p => p.Id == id).Get();
        if (!existingResponse.Models.Any())
        {
            return NotFound();
        }

        program.Id = id;
        var response = await _supabase.From<StudyProgram>().Update(program);
        return Ok(response.Models.FirstOrDefault());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,rector")]
    public async Task<IActionResult> DeleteProgram(long id)
    {
        await _supabase.From<StudyProgram>().Where(p => p.Id == id).Delete();
        return NoContent();
    }
}
