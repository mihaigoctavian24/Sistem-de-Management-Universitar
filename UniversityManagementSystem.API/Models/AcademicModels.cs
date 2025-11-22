using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UniversityManagementSystem.API.Models;

[Table("faculties")]
public class Faculty : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("dean_id")]
    public Guid? DeanId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    
    // Navigation properties - not mapped to DB columns directly
    public Profile? Dean { get; set; }
}

[Table("programs")]
public class StudyProgram : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("faculty_id")]
    public long? FacultyId { get; set; }

    [Column("study_type")]
    public string? StudyType { get; set; } // Bachelor, Master, PhD

    [Column("duration")]
    public int? Duration { get; set; } // Years

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    
    // Navigation properties
    public Faculty? Faculty { get; set; }
}

[Table("groups")]
public class Group : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("program_id")]
    public long? ProgramId { get; set; }

    [Column("year")]
    public int? Year { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    
    // Navigation properties
    public StudyProgram? Program { get; set; }
}

public class ProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

// DTOs for API responses
public class FacultyDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? DeanId { get; set; }
    public string? DeanName { get; set; }
    public int ProgramsCount { get; set; }
}

public class ProgramDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long? FacultyId { get; set; }
    public string? FacultyName { get; set; }
    public string? StudyType { get; set; }
    public int? Duration { get; set; }
    public int GroupsCount { get; set; }
}

public class GroupDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long? ProgramId { get; set; }
    public string? ProgramName { get; set; }
    public int? Year { get; set; }
    public int StudentsCount { get; set; }
}

// Request DTOs for create/update
public class CreateFacultyRequest
{
    public string Name { get; set; } = string.Empty;
    public Guid? DeanId { get; set; }
}

public class CreateProgramRequest
{
    public string Name { get; set; } = string.Empty;
    public long FacultyId { get; set; }
    public string StudyType { get; set; } = "Bachelor";
    public int Duration { get; set; } = 3;
}

public class CreateGroupRequest
{
    public string Name { get; set; } = string.Empty;
    public long ProgramId { get; set; }
    public int Year { get; set; } = 1;
}
