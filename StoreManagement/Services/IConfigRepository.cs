using StoreManagement.Models;

namespace StoreManagement.Services
{
    public interface IConfigRepository : IRepository<Config>
    {
        Task<Config> GetByKeyAsync(string key);
        Task SetConfigAsync(string key, string value);
    }
}
