using Application.Extentions;
using Application.Interfaces.Context;
using Application.Interfaces.Identity;
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
    public class RoleService : BaseService<Role>, IRoleService
    {
        #region Constructor
        private readonly IDbContext context;
        private readonly ILogger<RoleService> logger;
        public RoleService(IDbContext _context,
            ILogger<RoleService> _logger) : base(_context)
        {
            context = _context;
            logger = _logger;
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