﻿using System.Linq.Expressions;

namespace StoreManagement.Services
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
                int pageIndex,
                int pageSize,
                Expression<Func<T, bool>> predicate = null,
                Expression<Func<T, object>> orderBy = null,
                bool isDescending = false);
        Task UpdateSpecificPropertiesAsync(Expression<Func<T, bool>> predicate, Action<T> updateAction);
    }
}
