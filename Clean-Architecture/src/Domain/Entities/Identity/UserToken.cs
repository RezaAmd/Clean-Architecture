using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class UserToken : IdentityUserToken<string>
    {
        public virtual User User { get; set; }
    }
}