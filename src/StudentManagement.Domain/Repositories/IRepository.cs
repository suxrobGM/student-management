namespace StudentManagement.Domain.Repositories;

public interface IRepository<T> where T : class, IAggregateRoot
{
    T? Get(ulong id);
    IList<T> GetAll();
    void Add(T entity);
    void Update(ulong id, T entity);
    void Delete(ulong id);
}