using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UniversityManagementSystem.API.Models;

[Table("professors")]
public class Professor : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("faculty_id")]
    public long? FacultyId { get; set; }
}
