namespace UniversityManagementSystem.Client.Models;

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

public class ProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    
    public string FullName => $"{FirstName} {LastName}";
}
