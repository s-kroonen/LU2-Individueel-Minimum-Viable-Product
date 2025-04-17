using WebApi.api.Models;

namespace WebApi.api.Interfaces
{
    public interface IEnvironment2DRepository<T>
    {
        Task<Environment2D> InsertAsync(Environment2D environment2D, string userId);
        Task<Environment2D?> ReadAsync(Guid environment2DGuid);
        Task<IEnumerable<Environment2D>> ReadAllAsync();
        Task<IEnumerable<Environment2D>> ReadByUserAsync(string userId);
        Task UpdateAsync(Environment2D environment2D);
        Task DeleteAsync(Guid environment2D);
    }
}
