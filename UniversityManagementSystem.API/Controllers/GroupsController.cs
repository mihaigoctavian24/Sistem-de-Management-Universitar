using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public GroupsController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet]
    public async Task<ActionResult<List<GroupDto>>> GetGroups()
    {
        try
        {
            var response = await _supabase.From<Group>()
                .Get();
            
            var groups = response.Models;
            
            // Get programs
            var programsResponse = await _supabase.From<StudyProgram>()
                .Get();
            var programs = programsResponse.Models;
            
            var groupDtos = groups.Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                ProgramId = g.ProgramId,
                ProgramName = g.ProgramId.HasValue 
                    ? programs.FirstOrDefault(p => p.Id == g.ProgramId)?.Name 
                    : null,
                Year = g.Year,
                StudentsCount = 0 // Placeholder until we implement students module fully
            }).ToList();

            return Ok(groupDtos);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving groups: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDto>> GetGroup(long id)
    {
        try
        {
            var response = await _supabase.From<Group>()
                .Where(g => g.Id == id)
                .Single();
            
            if (response == null)
                return NotFound();

            var programResponse = await _supabase.From<StudyProgram>()
                .Where(p => p.Id == response.ProgramId)
                .Single();

            var groupDto = new GroupDto
            {
                Id = response.Id,
                Name = response.Name,
                ProgramId = response.ProgramId,
                ProgramName = programResponse?.Name,
                Year = response.Year,
                StudentsCount = 0
            };

            return Ok(groupDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving group: {ex.Message}");
        }
    }

    [HttpGet("program/{programId}")]
    public async Task<ActionResult<List<GroupDto>>> GetGroupsByProgram(long programId)
    {
        try
        {
            var response = await _supabase.From<Group>()
                .Where(g => g.ProgramId == programId)
                .Get();
            
            var groups = response.Models;
            
            var programResponse = await _supabase.From<StudyProgram>()
                .Where(p => p.Id == programId)
                .Single();

            var groupDtos = groups.Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                ProgramId = g.ProgramId,
                ProgramName = programResponse?.Name,
                Year = g.Year,
                StudentsCount = 0
            }).ToList();

            return Ok(groupDtos);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving groups by program: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<GroupDto>> CreateGroup([FromBody] CreateGroupRequest request)
    {
        try
        {
            var group = new Group
            {
                Name = request.Name,
                ProgramId = request.ProgramId,
                Year = request.Year
            };

            var response = await _supabase.From<Group>()
                .Insert(group);
            
            var created = response.Models.FirstOrDefault();
            if (created == null)
                return BadRequest("Failed to create group");

            var groupDto = new GroupDto
            {
                Id = created.Id,
                Name = created.Name,
                ProgramId = created.ProgramId,
                Year = created.Year,
                StudentsCount = 0
            };

            return CreatedAtAction(nameof(GetGroup), new { id = created.Id }, groupDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error creating group: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<GroupDto>> UpdateGroup(long id, [FromBody] CreateGroupRequest request)
    {
        try
        {
            var group = new Group
            {
                Id = id,
                Name = request.Name,
                ProgramId = request.ProgramId,
                Year = request.Year
            };

            var response = await _supabase.From<Group>()
                .Where(g => g.Id == id)
                .Update(group);
            
            var updated = response.Models.FirstOrDefault();
            if (updated == null)
                return NotFound();

            var groupDto = new GroupDto
            {
                Id = updated.Id,
                Name = updated.Name,
                ProgramId = updated.ProgramId,
                Year = updated.Year
            };

            return Ok(groupDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating group: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> DeleteGroup(long id)
    {
        try
        {
            // Check if group has students (if we had a generic way to check)
            // For now, we'll just allow delete but in production we should check
            
            await _supabase.From<Group>()
                .Where(g => g.Id == id)
                .Delete();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Error deleting group: {ex.Message}");
        }
    }
}
