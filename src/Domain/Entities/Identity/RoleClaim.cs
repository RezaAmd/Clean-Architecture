using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public virtual Role Role { get; set; }
    }
}