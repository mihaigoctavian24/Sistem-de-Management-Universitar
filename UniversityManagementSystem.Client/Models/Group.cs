using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UniversityManagementSystem.Client.Models;

[Table("groups")]
public class Group : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("program_id")]
    public long ProgramId { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
