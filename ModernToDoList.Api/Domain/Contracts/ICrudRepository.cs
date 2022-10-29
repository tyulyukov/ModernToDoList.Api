namespace ModernToDoList.Api.Repositories;

public interface ICrudRepository<T>
{
    Task<bool> CreateAsync(T obj);

    Task<T?> GetAsync(Guid id);

    Task<IEnumerable<T>> GetAllAsync();

    Task<bool> UpdateAsync(T obj);

    Task<bool> DeleteAsync(Guid id);
}