using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagementSystem.API.Models;

namespace UniversityManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public ReportsController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    [HttpGet("student/{studentId}/performance")]
    [Authorize(Roles = "student,professor,admin,dean,rector,secretary")]
    public async Task<ActionResult<StudentPerformanceDto>> GetStudentPerformance(Guid studentId)
    {
        try
        {
            // Fetch grades for the student
            var gradesResponse = await _supabase.From<Grade>()
                .Where(g => g.StudentId == studentId)
                .Get();
            var grades = gradesResponse.Models;

            if (!grades.Any())
            {
                return Ok(new StudentPerformanceDto());
            }

            // Fetch courses to get names and credits
            var courseIds = grades.Select(g => g.CourseId).Distinct().ToList();
            var coursesResponse = await _supabase.From<Course>()
                .Get(); // Fetching all courses for now as Where(c => courseIds.Contains(c.Id)) might be tricky with Supabase client limitations
            
            var courses = coursesResponse.Models.Where(c => courseIds.Contains(c.Id)).ToDictionary(c => c.Id, c => c);

            var performance = new StudentPerformanceDto
            {
                GradesByCourse = grades.Select(g => new CourseGradeDto
                {
                    CourseId = g.CourseId,
                    CourseName = courses.ContainsKey(g.CourseId) ? courses[g.CourseId].Name : "Unknown Course",
                    Grade = g.Value,
                    Credits = courses.ContainsKey(g.CourseId) ? (courses[g.CourseId].Credits ?? 0) : 0
                }).ToList()
            };

            if (performance.GradesByCourse.Any())
            {
                performance.AverageGrade = performance.GradesByCourse.Average(g => g.Grade);
                performance.TotalCredits = performance.GradesByCourse.Where(g => g.Grade >= 5).Sum(g => g.Credits); // Assuming 5 is passing grade
            }

            return Ok(performance);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving student performance: {ex.Message}");
        }
    }

    [HttpGet("student/{studentId}/attendance")]
    [Authorize(Roles = "student,professor,admin,dean,rector,secretary")]
    public async Task<ActionResult<AttendanceStatsDto>> GetStudentAttendance(Guid studentId)
    {
        try
        {
            var attendanceResponse = await _supabase.From<Attendance>()
                .Where(a => a.StudentId == studentId)
                .Get();
            var attendanceRecords = attendanceResponse.Models;

            var stats = new AttendanceStatsDto
            {
                PresentCount = attendanceRecords.Count(a => a.Status == "Present"),
                AbsentCount = attendanceRecords.Count(a => a.Status == "Absent"),
                ExcusedCount = attendanceRecords.Count(a => a.Status == "Excused")
            };

            if (attendanceRecords.Any())
            {
                stats.AttendancePercentage = (double)stats.PresentCount / attendanceRecords.Count * 100;
            }

            // Group by course
            var courseIds = attendanceRecords.Select(a => a.CourseId).Distinct().ToList();
            var coursesResponse = await _supabase.From<Course>().Get();
            var courses = coursesResponse.Models.Where(c => courseIds.Contains(c.Id)).ToDictionary(c => c.Id, c => c);

            stats.AttendanceByCourse = attendanceRecords
                .GroupBy(a => a.CourseId)
                .Select(g =>
                {
                    var total = g.Count();
                    var present = g.Count(a => a.Status == "Present");
                    return new CourseAttendanceDto
                    {
                        CourseId = g.Key,
                        CourseName = courses.ContainsKey(g.Key) ? courses[g.Key].Name : "Unknown Course",
                        PresentCount = present,
                        AbsentCount = g.Count(a => a.Status == "Absent"),
                        ExcusedCount = g.Count(a => a.Status == "Excused"),
                        AttendancePercentage = total > 0 ? (double)present / total * 100 : 0
                    };
                }).ToList();

            return Ok(stats);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving student attendance: {ex.Message}");
        }
    }

    [HttpGet("group/{groupId}/performance")]
    [Authorize(Roles = "professor,admin,dean,rector,secretary")]
    public async Task<ActionResult<GroupPerformanceDto>> GetGroupPerformance(long groupId)
    {
        try
        {
            // Fetch group details
            var groupResponse = await _supabase.From<Group>()
                .Where(g => g.Id == groupId)
                .Get();
            var group = groupResponse.Models.FirstOrDefault();

            if (group == null)
            {
                return NotFound("Group not found");
            }

            // Fetch students in group
            var studentsResponse = await _supabase.From<Student>()
                .Where(s => s.GroupId == groupId)
                .Get();
            var students = studentsResponse.Models;

            // Fetch profiles for names
            var profilesResponse = await _supabase.From<Profile>().Get(); // Optimally filter by student IDs
            var profiles = profilesResponse.Models.ToDictionary(p => p.Id, p => p);

            var groupPerformance = new GroupPerformanceDto
            {
                GroupId = groupId,
                GroupName = group.Name,
                StudentCount = students.Count
            };

            // Fetch all grades (this might be heavy, ideally filter by student IDs if possible in one go)
            var gradesResponse = await _supabase.From<Grade>().Get();
            var allGrades = gradesResponse.Models;

            foreach (var student in students)
            {
                var studentGrades = allGrades.Where(g => g.StudentId == student.Id).ToList();
                var avg = studentGrades.Any() ? studentGrades.Average(g => g.Value) : 0;

                groupPerformance.StudentPerformances.Add(new StudentGradeSummaryDto
                {
                    StudentId = student.Id,
                    StudentName = profiles.ContainsKey(student.Id) ? $"{profiles[student.Id].FirstName} {profiles[student.Id].LastName}" : "Unknown",
                    AverageGrade = avg
                });
            }

            if (groupPerformance.StudentPerformances.Any(s => s.AverageGrade > 0))
            {
                groupPerformance.AverageGrade = groupPerformance.StudentPerformances
                    .Where(s => s.AverageGrade > 0)
                    .Average(s => s.AverageGrade);
            }

            return Ok(groupPerformance);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving group performance: {ex.Message}");
        }
    }
}
