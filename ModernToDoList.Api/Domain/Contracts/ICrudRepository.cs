namespace ModernToDoList.Api.Repositories;

public interface ICrudRepository<T, TDto>
{
    Task<bool> CreateAsync(T obj);

    Task<TDto?> GetAsync(Guid id);

    Task<IEnumerable<TDto>> GetAllAsync();

    Task<bool> UpdateAsync(T obj);

    Task<bool> DeleteAsync(Guid id);
}