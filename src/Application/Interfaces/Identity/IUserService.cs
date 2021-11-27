using Application.Models;
using Domain.Entities.Identity;
using Domain.Enums;
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
        #region base
        Task<IdentityResult> CreateAsync(User user);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteAsync(User user);
        Task<User> FindByIdAsync(string userId);
        Task<User> FindByNameAsync(string userName);
        Task<User> FindByEmailAsync(string email);
        string NormalizeName(string name);
        string NormalizeEmail(string email);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles);
        Task<IdentityResult> RemoveFromRoleAsync(User user, string role);
        Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
        Task<IdentityResult> AddPasswordAsync(User user, string password);
        Task<IdentityResult> RemovePasswordAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
        Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<User> store, User user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<bool> IsLockedOutAsync(User user);
        Task<IdentityResult> SetLockoutEnabledAsync(User user, bool enabled);
        #endregion

        #region Custom
        Task<PaginatedList<TDestination>> GetAllAsync<TDestination>(TypeAdapterConfig config = null, int page = 1, int pageSize = 20,
            bool withRoles = false, string keyword = default, bool tracking = false, CancellationToken cancellationToken = new CancellationToken());
        Task<User> FindByIdentityAsync(string identity, bool asNoTracking = false, bool withRoles = false,
            bool withClaims = false, bool withTokens = false, TypeAdapterConfig config = null);
        (Result Status, string Token) GenerateJwtToken(User user, DateTime? expire = default);
        string GenerateOtp(string phoneNumber, HttpContext httpContext);
        (Result Result, string PhoneNumber) VerifyOtp(string code, HttpContext httpContext);
        #endregion
    }
}