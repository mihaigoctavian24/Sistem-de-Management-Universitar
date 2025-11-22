using System;
using System.Text.Json.Serialization;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UniversityManagementSystem.Client.Models;

[Table("attendance")]
public class Attendance : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("course_id")]
    public long CourseId { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("status")]
    public string Status { get; set; } = "Absent";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
