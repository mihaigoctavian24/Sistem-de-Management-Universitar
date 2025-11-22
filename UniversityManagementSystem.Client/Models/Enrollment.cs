using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UniversityManagementSystem.Client.Models;

[Table("enrollments")]
public class Enrollment : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("program_id")]
    public long ProgramId { get; set; }

    [Column("academic_year")]
    public string AcademicYear { get; set; } = string.Empty;

    [Column("status")]
    public string Status { get; set; } = "pending";

    [Column("enrollment_date")]
    public DateTime? EnrollmentDate { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
