
using Microsoft.AspNetCore.Identity;

namespace Domain.Entity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public Guid MainRoleId { get; set; }
        public int CountEnter { get; set; }
        public int GenerationCount { get; set; } = 1;
        public DateTime LastActive { get; set; }

        public void UpdateActive()
        {
            LastActive = DateTime.UtcNow.AddHours(5);
            CountEnter += 1;
        }
    }
}
