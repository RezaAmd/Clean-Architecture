using System;
using System.Collections.Generic;

namespace Domain.Entities.Identity
{
    public class User
    {
        #region Constructors
        User() { }

        public User(string username)
        {

            Username = username;
            NormalizedUsername = username.ToUpper();
        }

        public User(string username, string phoneNumber)
        {
            Username = username;
            NormalizedUsername = username.ToUpper();
            PhoneNumber = phoneNumber;

            JoinedDate = DateTime.Now;
        }
        #endregion

#nullable disable
        public string Id { get; set; }
        public string Username { get; set; }
        public string NormalizedUsername { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
#nullable enable
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Avatar { get; set; }
        public DateTime? LockoutEnd { get; set; }
#nullable disable
        public bool LockedOutEnabled { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime JoinedDate { get; set; }

        #region Relation
        public virtual ICollection<UserRole> UserRoles { get; set; }
        #endregion
    }
}