using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    public class UserRole
    {
        #region Constructors
        UserRole() { }

        public UserRole(string userId, string roleId)
        {
            UserId = userId;
            RoleId = roleId;
            AssignedDateTime = DateTime.Now;
        }
        #endregion
        public DateTime AssignedDateTime { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Role")]
        public string RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}