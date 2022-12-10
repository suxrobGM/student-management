using System.Linq.Expressions;

namespace StudentManagement.Domain.Persistence;

public interface IRepository<T> where T : class, IAggregateRoot
{
    IQueryable<T> Query(Expression<Func<T, bool>>? predicate = default);
    Task<T?> GetAsync(ulong id);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(ulong id, T entity);
    Task DeleteAsync(ulong id);
}