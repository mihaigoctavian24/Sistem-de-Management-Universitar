using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UniversityManagementSystem.API.Models;

[Table("grades")]
public class Grade : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("course_id")]
    public long CourseId { get; set; }

    [Column("value")]
    public int Value { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
