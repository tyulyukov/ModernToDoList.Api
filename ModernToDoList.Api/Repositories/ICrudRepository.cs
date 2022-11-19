namespace ModernToDoList.Api.Repositories;

public interface ICrudRepository<T>
{
    Task<bool> CreateAsync(T obj, CancellationToken ct);

    Task<T?> GetAsync(string id, CancellationToken ct);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);

    Task<bool> UpdateAsync(T obj, CancellationToken ct);

    Task<bool> DeleteAsync(string id, CancellationToken ct);
}