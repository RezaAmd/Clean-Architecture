using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public virtual User User { get; set; }
    }
}