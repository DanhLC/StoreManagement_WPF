namespace StoreManagement.Core.Interfaces.Services
{
    public interface IConfigRepository : IRepository<Config>
    {
        Task<Config> GetByKeyAsync(string key);
        Task SetConfigAsync(string key, string value);
    }
}
