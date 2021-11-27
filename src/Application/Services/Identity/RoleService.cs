using Application.Extentions;
using Application.Interfaces.Context;
using Application.Models;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Identity
{
    public class RoleService : RoleManager<Role>
    {
        #region Constructor
        private readonly IRoleStore<Role> roleStore;
        private readonly IEnumerable<IRoleValidator<Role>> roleValidator;
        private readonly ILookupNormalizer keyNormalizer;
        private readonly ErrorDescriber error;
        private readonly ILogger<RoleManager<Role>> logger;
        private readonly IDbContext context;
        public RoleService(IRoleStore<Role> _roleStore,
            IEnumerable<IRoleValidator<Role>> _roleValidator,
            ILookupNormalizer _keyNormalizer,
            ErrorDescriber _error,
            ILogger<RoleManager<Role>> _logger,
            IDbContext _context)
            : base(_roleStore,
                  _roleValidator,
                  _keyNormalizer,
                  _error,
                  _logger)
        {
            roleStore = _roleStore;
            roleValidator = _roleValidator;
            keyNormalizer = _keyNormalizer;
            error = _error;
            logger = _logger;
            context = _context;
        }

        #endregion

        public async Task<Result> CreateRangeAsync(List<Role> roles)
        {
            try
            {
                context.Roles.AddRange(roles);
                if (Convert.ToBoolean(await context.SaveChangesAsync(new CancellationToken())))
                    return Result.Success;
            }
            catch (DbUpdateException ex)
            {
                return Result.Failed(new() { ex.HandleDbExtentionFilter() });
            }
            return Result.Failed();
        }

        public async Task<IList<Role>> GetAllAsync(int page = 1, int pageSize = 25)
        {
            return await context.Roles.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}