using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UniversityManagementSystem.Client.Models;

[Table("faculties")]
public class Faculty : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("dean_id")]
    public Guid? DeanId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
