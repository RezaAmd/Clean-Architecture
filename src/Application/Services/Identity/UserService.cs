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
    public class UserService : UserManager<User>, IUserService
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
            : base(_store, _options, _passwordHasher, _userValidators, _passwordValidators,
                _normalizer, _errors, _serviceProvider, _logger)
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
                init = init.Where(u => keyword.Contains(u.NormalizedUserName) || keyword.Contains(u.Name)
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
            bool withClaims = false, bool withTokens = false, TypeAdapterConfig config = null)
        {
            var init = context.Users.Where(u => u.NormalizedUserName == identity.ToUpper()
            || u.PhoneNumber == identity
            || u.NormalizedEmail == identity.ToUpper()
            || u.Id == identity);
            #region include's
            if (asNoTracking)
                init = init.AsNoTracking();
            if (withRoles)
                init = init.Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role);
            if (withClaims)
                init = init.Include(u => u.Claims);
            if (withTokens)
                init = init.Include(u => u.Tokens);
            #endregion
            return await init.FirstOrDefaultAsync();
        }

        public (Result Status, string Token) GenerateJwtToken(User user, DateTime? expire = default)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            if (user.UserRoles != null)
                foreach (var role in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
                }
            var jwtResult = jwtService.GenerateToken(claims, expire);
            return jwtResult;
        }

        Task<PasswordVerificationResult> IUserService.VerifyPasswordAsync(IUserPasswordStore<User> store, User user, string password)
        {
            return VerifyPasswordAsync(store, user, password);
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