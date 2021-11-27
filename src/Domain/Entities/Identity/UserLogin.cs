using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class UserLogin : IdentityUserLogin<string>
    {
        public virtual User User { get; set; }
    }
}