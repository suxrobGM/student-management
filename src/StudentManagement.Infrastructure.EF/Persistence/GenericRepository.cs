using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain;
using StudentManagement.Infrastructure.EF.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StudentManagement.Infrastructure.EF.Persistence;

internal class GenericRepository<T> : IRepository<T> where T : class, IAggregateRoot
{
    private readonly DatabaseContext _context;

    public GenericRepository(DatabaseContext context)
    {
        _context = context;
    }

    public Task AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        return _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ulong id)
    {
        var entity = _context.Set<T>().FirstOrDefault(x => x.Id == id);

        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public Task<T?> GetAsync(ulong id)
    {
        return _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<T>> GetAllAsync()
    {
        return _context.Set<T>().ToListAsync();
    }

    public IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate == default ? _context.Set<T>() : _context.Set<T>().Where(predicate);
    }

    public Task UpdateAsync(ulong id, T entity)
    {
        _context.Set<T>().Update(entity);
        return _context.SaveChangesAsync();
    }
}
