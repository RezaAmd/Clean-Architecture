using Application.Models;
using Domain.Entities.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Identity
{
    public interface IRoleService
    {
        #region Base
        Task<Role?> FindByIdAsync(CancellationToken cancellationToken = new CancellationToken(),params object?[]? id);

        Task<Result> CreateAsync(Role entry, CancellationToken cancellationToken = new CancellationToken());

        Task<Result> DeleteAsync(Role role, CancellationToken cancellationToken = new CancellationToken());

        Task<Result> UpdateAsync(Role entry, CancellationToken cancellationToken = new CancellationToken());
        #endregion
    }
}