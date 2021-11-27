using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        #region Constructors
        User() { }
        public User(string username)
        {
            UserName = username;
            NormalizedUserName = username.ToUpper();
        }

        public User(string username, string phoneNumber)
        {
            UserName = username;
            NormalizedUserName = username.ToUpper();
            PhoneNumber = phoneNumber;

            JoinedDate = DateTime.Now;
            isBanned = false;
        }

        #endregion

#nullable enable
        public string? Name { get; set; }
        public string? Surname { get; set; }
#nullable disable
        public DateTime JoinedDate { get; set; }
        public bool IsBanned { get; set; }

        #region Relation
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        #endregion
    }
}