namespace UniversityManagementSystem.Client.Models;

public class StudentPerformanceDto
{
    public double AverageGrade { get; set; }
    public int TotalCredits { get; set; }
    public List<CourseGradeDto> GradesByCourse { get; set; } = new();
}

public class CourseGradeDto
{
    public long CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int Grade { get; set; }
    public int Credits { get; set; }
}

public class AttendanceStatsDto
{
    public int PresentCount { get; set; }
    public int AbsentCount { get; set; }
    public int ExcusedCount { get; set; }
    public double AttendancePercentage { get; set; }
    public List<CourseAttendanceDto> AttendanceByCourse { get; set; } = new();
}

public class CourseAttendanceDto
{
    public long CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int PresentCount { get; set; }
    public int AbsentCount { get; set; }
    public int ExcusedCount { get; set; }
    public double AttendancePercentage { get; set; }
}

public class GroupPerformanceDto
{
    public long GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public double AverageGrade { get; set; }
    public int StudentCount { get; set; }
    public List<StudentGradeSummaryDto> StudentPerformances { get; set; } = new();
}

public class StudentGradeSummaryDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public double AverageGrade { get; set; }
}
