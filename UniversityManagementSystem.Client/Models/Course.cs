using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UniversityManagementSystem.Client.Models;

[Table("courses")]
public class Course : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("professor_id")]
    public Guid? ProfessorId { get; set; }

    [Column("program_id")]
    public long? ProgramId { get; set; }

    [Column("semester")]
    public int? Semester { get; set; }

    [Column("credits")]
    public int? Credits { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
