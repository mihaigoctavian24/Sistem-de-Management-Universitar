namespace UniversityManagementSystem.Client.Models;

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
