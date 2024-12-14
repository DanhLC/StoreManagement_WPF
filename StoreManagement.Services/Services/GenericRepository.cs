using Microsoft.EntityFrameworkCore;
using StoreManagement.Core.Interfaces.Services;
using StoreManagement.Infrastructure.Data;
using System.Linq.Expressions;

namespace StoreManagement.Services
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSpecificPropertiesAsync(
            Expression<Func<T, bool>> predicate,
            Action<T> updateAction)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);

            if (entity == null)  throw new InvalidOperationException("Entity not found.");

            updateAction(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteManyAsync(Func<T, bool> predicate)
        {
            var entities = _dbSet.Where(predicate).ToList();

            if (entities.Any())
            {
                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteOneAsync(Func<T, bool> predicate)
        {
            var entity = _dbSet.FirstOrDefault(predicate);

            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>> orderBy = null,
            bool isDescending = false)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
