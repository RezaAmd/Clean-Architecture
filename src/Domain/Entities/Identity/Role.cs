using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Identity
{
    public class Role : IdentityRole
    {
        #region Constructors
        Role() { }
        public Role(string name) : base(name)
        {
            Id = Guid.NewGuid().ToString();
            NormalizedName = name.ToUpper();
        }
        #endregion

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }
}