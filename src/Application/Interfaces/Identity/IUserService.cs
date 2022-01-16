using Application.Models;
using Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Identity
{
    public interface IUserService
    {
        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user">New user model object.</param>
        Task<Result> CreateAsync(User user, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Create a new user with password.
        /// </summary>
        /// <param name="user">New user model object.</param>
        /// <param name="password">User account password.</param>
        Task<Result> CreateAsync(User user, string password, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Update a specific user.
        /// </summary>
        /// <param name="user">User model object for update.</param>
        Task<Result> UpdateAsync(User user, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Delete a specific user.
        /// </summary>
        /// <param name="user">User model object.</param>
        Task<Result> DeleteAsync(User user, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Find a specific
        /// </summary>
        /// <param name="userId">User id for find.</param>
        Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Find a specific user by username.
        /// </summary>
        /// <param name="username">Username for find.</param>
        Task<User> FindByNameAsync(string username, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Find a specific user by email.
        /// </summary>
        /// <param name="email">email for search.</param>
        Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Add user to a role.
        /// </summary>
        /// <param name="user">User model object.</param>
        /// <param name="role">Role name for assign to user.</param>
        Task<Result> AddToRoleAsync(User user, Role role, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Assign a specific user to list of roles.
        /// </summary>
        /// <param name="user">User model object.</param>
        /// <param name="roles">Roles name for assign to user.</param>
        Task<Result> AddToRolesAsync(User user, IEnumerable<string> roles);

        /// <summary>
        /// Remove an role from user roles.
        /// </summary>
        /// <param name="user">User model object.</param>
        /// <param name="role">Role name.</param>
        Task<Result> RemoveFromRoleAsync(User user, Role role, CancellationToken cancellationToken = new CancellationToken());
        Task<Result> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
        Task<Result> AddPasswordAsync(User user, string password, CancellationToken cancellationToken = new CancellationToken());
        Task<Result> RemovePasswordAsync(User user, CancellationToken cancellationToken = new CancellationToken());
        Task<Result> ResetPasswordAsync(User user, string newPassword, CancellationToken cancellationToken = new CancellationToken());
        Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<User> store, User user, string password);
        bool CheckPasswordAsync(User user, string password);
        bool IsLockedOutAsync(User user);
        Task<Result> SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken = new CancellationToken());
        Task<PaginatedList<TDestination>> GetAllAsync<TDestination>(TypeAdapterConfig config = null, int page = 1, int pageSize = 20,
            bool withRoles = false, string keyword = default, bool tracking = false, CancellationToken cancellationToken = new CancellationToken());
        Task<User> FindByIdentityAsync(string identity, bool asNoTracking = false, bool withRoles = false,
            TypeAdapterConfig config = null);
        (Result Status, string Token) GenerateJwtToken(User user, DateTime? expire = default);
        string GenerateOtp(string phoneNumber, HttpContext httpContext);
        (Result Result, string PhoneNumber) VerifyOtp(string code, HttpContext httpContext);
    }
}