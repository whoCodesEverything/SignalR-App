using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatApp.Core.DataAccess.EFEntity
{
    public class EfRepositoryBase<T> : IEntityRepository<T> where T : BaseEntity, new()
    {
        protected readonly DbContext _context;

        public EfRepositoryBase(DbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            return filter == null
                ? await _context.Set<T>().ToListAsync()
                : await _context.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().Where(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
