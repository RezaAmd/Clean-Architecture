using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class UserRole : IdentityUserRole<string>
    {
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}