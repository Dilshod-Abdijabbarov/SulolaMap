
using Domain.Entity;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class PersonDto
{
    public Guid Id { get; set; }
    public Guid? ParentSpouseId { get; set; }
    public string? Pinfl { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public int Order { get; set; } = 1;
    // shajara tartibi.1-farzand,2-farzand, va hakazo.
    public int GenerationLevel { get; set; } = 1;
    // avlod shajarasi.1-avlod 2-avlod, 3,3,4,5,6,7 shu kabi
    public string? PhotoUrl { get; set; }
    public long? PhoneNumber { get; set; }
    public bool IsAlive { get; set; } = true;//tirik yoki vafot etganligini bildiradi
    public DateTime? DeathDate { get; set; }
    public string? BirthPlace { get; set; }
    public string? Biography { get; set; }
    public string? TelegramLink { get; set; }
    public string? InstagramLink { get; set; }
    public Guid CreatedBy { get; set; }
    public string? Description { get; set; }

    // Agar bu kishi ER bo'lsa, uning xotinlari bilan bog'liqliklari
    [InverseProperty("Husband")]
    public virtual ICollection<Spouse> MarriagesAsHusband { get; set; }

    // Agar bu kishi XOTIN bo'lsa, uning erlari bilan bog'liqliklari
    [InverseProperty("Wife")]
    public virtual ICollection<Spouse> MarriagesAsWife { get; set; }
}
