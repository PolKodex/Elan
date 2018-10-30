using System.Linq;
using System.Threading.Tasks;
using Elan.Data.Contracts;

namespace Elan.Data.Services
{
    public class DataService: IDataService
    {
        private readonly ElanDbContext _dbContext;

        public DataService(ElanDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> GetSet<T>() where T: class
        {
            return _dbContext.Set<T>();
        }

        public async Task SaveDbAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
