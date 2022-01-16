using Application.Extentions;
using Application.Interfaces;
using Application.Interfaces.Context;
using Application.Interfaces.Identity;
using Application.Models;
using Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Identity
{
    public class UserService : IUserService
    {
        #region Constructor
        private readonly IUserStore<User> store;
        private readonly IOptions<IdentityOptions> options;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IEnumerable<IUserValidator<User>> userValidators;
        private readonly IEnumerable<IPasswordValidator<User>> passwordValidators;
        private readonly ILookupNormalizer normalizer;
        private readonly ErrorDescriber errors;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<UserManager<User>> logger;
        private readonly IDbContext context;
        private readonly IJwtService jwtService;
        private readonly string passwordHashSalt = "3T9$Ss^a3g27P";

        public UserService(IUserStore<User> _store,
            IOptions<IdentityOptions> _options,
            IPasswordHasher<User> _passwordHasher,
            IEnumerable<IUserValidator<User>> _userValidators,
            IEnumerable<IPasswordValidator<User>> _passwordValidators,
            ILookupNormalizer _normalizer,
            ErrorDescriber _errors,
            IServiceProvider _serviceProvider,
            ILogger<UserManager<User>> _logger,
            IDbContext _context,
            IJwtService _jwtService)
        {
            store = _store;
            options = _options;
            passwordHasher = _passwordHasher;
            userValidators = _userValidators;
            passwordValidators = _passwordValidators;
            normalizer = _normalizer;
            errors = _errors;
            serviceProvider = _serviceProvider;
            logger = _logger;
            context = _context;
            jwtService = _jwtService;
        }
        #endregion

        public async Task<User> FindByIdAsync(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            return await context.Users.FindAsync(id, cancellationToken);
        }

        public async Task<Result> CreateAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            await context.Users.AddAsync(user, cancellationToken);
            if (Convert.ToBoolean(await context.SaveChangesAsync(cancellationToken)))
                return Result.Success;
            return Result.Failed();
        }

        public async Task<Result> CreateAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            user.PasswordHash = password.Encrypt(passwordHashSalt);
            await context.Users.AddAsync(user, cancellationToken);
            if (Convert.ToBoolean(await context.SaveChangesAsync(cancellationToken)))
                return Result.Success;
            return Result.Failed();
        }

        public async Task<Result> UpdateAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            context.Users.Update(user);
            if (Convert.ToBoolean(await context.SaveChangesAsync(cancellationToken)))
                return Result.Success;
            return Result.Failed();
        }

        public async Task<Result> DeleteAsync(User user, CancellationToken cancellationToken = default)
        {
            context.Users.Remove(user);
            if (Convert.ToBoolean(await context.SaveChangesAsync(cancellationToken)))
                return Result.Success;
            return Result.Failed();
        }

        public async Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken = new CancellationToken()) =>
            await context.Users.Where(u => u.Username == userName).FirstOrDefaultAsync(cancellationToken);

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = new CancellationToken()) =>
            await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);

        public async Task<Result> AddToRoleAsync(User user, Role role, CancellationToken cancellationToken = default)
        {
            var userRole = new UserRole(user.Id, role.Id);
            user.UserRoles.Add(userRole);
            if (Convert.ToBoolean(await context.SaveChangesAsync(cancellationToken)))
                return Result.Success;
            return Result.Failed();
        }

        public async Task<Result> AddToRolesAsync(User user, IEnumerable<string> roles)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> RemoveFromRoleAsync(User user, Role role, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<Result> RemoveFromRolesAsync(User user, IEnumerable<string> roles)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> AddPasswordAsync(User user, string password, CancellationToken cancellationToken = new CancellationToken())
        {
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = password.Encrypt(passwordHashSalt);
                return await UpdateAsync(user);
            }
            return Result.Failed(new List<Error>{
                new (742, "User already had a password.")
            });
        }

        public async Task<Result> RemovePasswordAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = null;
                return await UpdateAsync(user);
            }
            return Result.Failed(new List<Error>
            {
                new(471, "The user has no password.")
            });
        }

        public Task<Result> ResetPasswordAsync(User user, string newPassword, CancellationToken cancellationToken = new CancellationToken())
        {
            user.PasswordHash = newPassword.Encrypt(passwordHashSalt);
            return UpdateAsync(user, cancellationToken);
        }

        public Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<User> store, User user, string password)
        {
            throw new NotImplementedException();
        }

        public bool CheckPasswordAsync(User user, string password)
        {
            if (user.PasswordHash == password.Encrypt(passwordHashSalt))
                return true;
            return false;
        }

        public bool IsLockedOutAsync(User user)
        {
            return user.LockoutEnd.HasValue;
        }

        public Task<Result> SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken = new CancellationToken())
        {
            user.LockedOutEnabled = enabled;
            return UpdateAsync(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword">Search to username, name, surname, fathersName and identityLetter.</param>
        /// <param name="gender">Fillter as gender.</param>
        /// <param name="tracking">For range changes.</param>
        /// <returns>List of all users</returns>
        public async Task<PaginatedList<TDestination>> GetAllAsync<TDestination>(TypeAdapterConfig config = null, int page = 1, int pageSize = 20,
            bool withRoles = false, string keyword = null, bool tracking = false, CancellationToken cancellationToken = new CancellationToken())
        {
            var init = context.Users.OrderBy(u => u.JoinedDate).AsQueryable();

            if (!tracking)
                init = init.AsNoTracking();
            // search
            if (!string.IsNullOrEmpty(keyword))
                init = init.Where(u => keyword.Contains(u.NormalizedUsername) || keyword.Contains(u.Name)
                 || keyword.Contains(u.Surname));

            // include roles
            if (withRoles)
                init = init.Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role);

            return await init
                .ProjectToType<TDestination>(config)
                .PaginatedListAsync(page, pageSize, cancellationToken);
        }

        /// <summary>
        /// Find by (Username or Phone number or Email)
        /// </summary>
        /// <param name="identity">identity for find</param>
        /// <returns>user</returns>
        public async Task<User> FindByIdentityAsync(string identity, bool asNoTracking = false, bool withRoles = false,
            TypeAdapterConfig config = null)
        {
            var init = context.Users.Where(u => u.NormalizedUsername == identity.ToUpper()
            || u.PhoneNumber == identity
            || u.NormalizedEmail == identity.ToUpper()
            || u.Id == identity);
            #region include's
            if (asNoTracking)
                init = init.AsNoTracking();
            if (withRoles)
                init = init.Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role);
            #endregion
            return await init.FirstOrDefaultAsync();
        }

        public (Result Status, string Token) GenerateJwtToken(User user, DateTime? expire = default)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            if (user.UserRoles != null)
                foreach (var role in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
                }
            var jwtResult = jwtService.GenerateToken(claims, expire);
            return jwtResult;
        }

        #region Otp and verify
        public string GenerateOtp(string phoneNumber, HttpContext httpContext)
        {
            string code = RandomGenerator.GenerateNumber();
            httpContext.Session.SetString("SigninOtp", (code + "." + phoneNumber).Encrypt());
            return code;
        }
        public (Result Result, string PhoneNumber) VerifyOtp(string code, HttpContext httpContext)
        {
            string otpSession = httpContext.Session.GetString("SigninOtp");
            if (otpSession != null)
            {
                var otpObject = otpSession.Decrypt().Split(".");
                if (otpObject[0] == code)
                {
                    httpContext.Session.Remove("SigninOtp");
                    return (Result.Success, otpObject[1]);
                }
            }
            return (Result.Failed(), null);
        }


        #endregion
    }
}