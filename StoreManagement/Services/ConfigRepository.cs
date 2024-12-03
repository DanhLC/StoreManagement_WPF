using Microsoft.EntityFrameworkCore;
using StoreManagement.Data;
using StoreManagement.Models;

namespace StoreManagement.Services
{
    public class ConfigRepository : GenericRepository<Config>, IConfigRepository
    {
        private readonly AppDbContext _context;

        public ConfigRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Config> GetByKeyAsync(string key)
        {
            return await _context.Config.FirstOrDefaultAsync(c => c.Key == key);
        }

        public async Task SetConfigAsync(string key, string value)
        {
            var config = await GetByKeyAsync(key);
            if (config == null)
            {
                config = new Config { Key = key, Value = value };
                await AddAsync(config);
            }
            else
            {
                config.Value = value;
                await UpdateAsync(config);
            }
        }
    }
}
