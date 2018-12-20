using Elan.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Elan.Data.Services
{
    public class DataService: IDataService
    {
        private readonly ElanDbContext _dbContext;

        public DataService(ElanDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<T> GetSet<T>() where T: class
        {
            return _dbContext.Set<T>();
        }

        public async Task SaveDbAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
