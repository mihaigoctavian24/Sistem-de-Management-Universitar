using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UniversityManagementSystem.Client.Models;

[Table("programs")]
public class StudyProgram : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("faculty_id")]
    public long FacultyId { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
