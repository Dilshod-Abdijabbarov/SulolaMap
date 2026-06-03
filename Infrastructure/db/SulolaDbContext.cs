
using Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.db
{
    public  class SulolaDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public SulolaDbContext(DbContextOptions<SulolaDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Spouse> Spouses { get; set; }
        public DbSet<Generation> Generations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SulolaDbContextSeed).Assembly).Seed();

            // Er orqali bog'liqlikni sozlash
            modelBuilder.Entity<Spouse>()
                .HasOne(s => s.Husband)
                .WithMany(p => p.MarriagesAsHusband)
                .HasForeignKey(s => s.HusbandId)
                .OnDelete(DeleteBehavior.Restrict); // O'chirishni cheklash

            // Xotin orqali bog'liqlikni sozlash
            modelBuilder.Entity<Spouse>()
                .HasOne(s => s.Wife)
                .WithMany(p => p.MarriagesAsWife)
                .HasForeignKey(s => s.WifeId)
                .OnDelete(DeleteBehavior.Restrict); // O'chirishni cheklash

            // Bolalar va nikoh bog'liqligi
            modelBuilder.Entity<Person>()
                .HasOne(p => p.BornFromMarriage)
                .WithMany(s => s.Childrens)
                .HasForeignKey(p => p.ParentSpouseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
