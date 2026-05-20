using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatApp.Core.DataAccess
{
    public interface IEntityRepository<T> where T : BaseEntity, new()
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
        Task<bool> AddAsync(T entity, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}
