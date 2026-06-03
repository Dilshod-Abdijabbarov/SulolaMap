
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;

[Table("generations")]
public class Generation
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; }

    [Column("description")]
    [MaxLength(200)]
    public string Description { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    // Amal qilish muddati
    [Column("expire_date")]
    public DateTime ExpireDate { get; set; } = DateTime.UtcNow.AddMonths(3);

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public ICollection<Person> Persons { get; set; }
}
