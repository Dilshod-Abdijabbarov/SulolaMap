
using System.Xml.Linq;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.db
{
    public static class SulolaDbContextSeed
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.SeedRoles();
            builder.SeedAuths();
        }

        private const string UserId = "b1d4d10b-2f84-4f08-a4b7-2b38600aa64e";
        private const string RoleSuperAdminId = "1c74cb58-ec8a-4373-81e9-311e39597493";
        private const string RoleAdminId = "225b97b8-ee88-4047-ab68-fe48464ddef4";
        private const string RoleUserId = "3565a6f5-1dda-4d68-9dc6-cf70148b920d";
        private static void SeedRoles(this ModelBuilder builder)
        {
            builder.Entity<ApplicationRole>(roles =>
            {
                roles.HasData(new ApplicationRole
                {
                    Id = Guid.Parse(RoleSuperAdminId),
                    Name = Roles.SuperAdmin,
                    NormalizedName = Roles.SuperAdmin.ToUpper(),
                    ConcurrencyStamp = "1"
                }, new ApplicationRole
                {
                    Id = Guid.Parse(RoleAdminId),
                    Name = Roles.Admin,
                    NormalizedName = Roles.Admin.ToUpper(),
                    ConcurrencyStamp = "2"
                }, new ApplicationRole
                {
                    Id = Guid.Parse(RoleUserId),
                    Name = Roles.User,
                    NormalizedName = Roles.User.ToUpper(),
                    ConcurrencyStamp = "3"
                });
            });
        }
        private static void SeedAuths(this ModelBuilder builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>(users =>
            {
                users.HasData(new ApplicationUser
                {
                    Id = Guid.Parse(UserId),
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "abdijabbarovdilshod@gmail.com",
                    NormalizedEmail = "abdijabbarovdilshod@gmail.com",
                    FirtName = "Dilshod",
                    LastName = "Abdijabbarov",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "admin$dr$7"),
                    SecurityStamp = string.Empty,
                    LastActive = DateTime.UtcNow.AddHours(5),
                    MainRoleId = Guid.Parse(RoleSuperAdminId)
                });
            });

            builder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = Guid.Parse(RoleSuperAdminId),
                UserId = Guid.Parse(UserId)
            },
            new IdentityUserRole<Guid>
            {
                RoleId = Guid.Parse(RoleAdminId),
                UserId = Guid.Parse(UserId)
            });
        }
    }
}
