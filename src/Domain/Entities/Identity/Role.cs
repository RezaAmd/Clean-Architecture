using System;
using System.Collections.Generic;

namespace Domain.Entities.Identity
{
    public class Role
    {
        #region Constructors
        Role() { }
        public Role(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name.Trim();
            NormalizedName = name.Trim().ToUpper();
        }
        #endregion

        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        #region Relations
        public virtual ICollection<UserRole> UserRoles { get; set; }
        #endregion
    }
}