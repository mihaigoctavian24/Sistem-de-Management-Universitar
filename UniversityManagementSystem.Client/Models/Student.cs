using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UniversityManagementSystem.Client.Models;

[Table("students")]
public class Student : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("group_id")]
    public long? GroupId { get; set; }

    [Column("registration_number")]
    public string? RegistrationNumber { get; set; }

    [Column("status")]
    public string Status { get; set; } = "active";

    [Column("enrollment_date")]
    public DateTime? EnrollmentDate { get; set; }
}
