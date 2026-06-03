using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entity
{
    [Table("persons")]
    public class Person
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("parent_spouse_id")]
        public Guid? ParentSpouseId { get; set; }

        [Column("pinfl")]
        public string? Pinfl { get; set; }

        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [Column("last_name")]
        public string LastName { get; set; }

        [Required]
        [Column("middle_name")]
        public string MiddleName { get; set; }

        [Required]
        [Column("birthdate")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Column("gender")]
        public Gender Gender { get; set; }

        [Required]
        [Column("child_order")]
        public int ChildOrder { get; set; } = 1;
        // shajara tartibi.1-farzand,2-farzand, va hakazo.

        [Required]
        [Column("generation_level")]
        public int GenerationLevel { get; set; } = 1;
        // avlod shajarasi.1-avlod 2-avlod, 3,3,4,5,6,7 shu kabi

        [Column("photo_url")]
        public string? PhotoUrl { get; set; }

        [Column("phone_number")]
        public long? PhoneNumber { get; set; }

        [Required]
        [Column("is_alive")]
        public bool IsAlive { get; set; } = true;//tirik yoki vafot etganligini bildiradi

        [Column("death_date")]
        public DateTime? DeathDate { get; set; }

        [Column("birth_place")]
        public string? BirthPlace { get; set; }

        [Column("biography")]
        public string? Biography { get; set; }

        [Column("telegram_link")]
        public string? TelegramLink { get; set; }

        [Column("instagram_link")]
        public string? InstagramLink { get; set; }

        [Column("created_by")]
        public Guid CreatedBy { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("generation_id")]
        public Guid? GenerationId { get; set; }

        // Agar bu kishi ER bo'lsa, uning xotinlari bilan bog'liqliklari
        [InverseProperty("Husband")]
        public virtual ICollection<Spouse> MarriagesAsHusband { get; set; }

        // Agar bu kishi XOTIN bo'lsa, uning erlari bilan bog'liqliklari
        [InverseProperty("Wife")]
        public virtual ICollection<Spouse> MarriagesAsWife { get; set; }

        // Bu kishi qaysi nikohdan (ota-onadan) tug'ilgani
        [ForeignKey("ParentSpouseId")]
        public virtual Spouse? BornFromMarriage { get; set; }

        public Generation Generation { get; set; }
    }
}
