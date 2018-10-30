using System.Linq;
using System.Threading.Tasks;

namespace Elan.Data.Contracts
{
    public interface IDataService
    {
        IQueryable<T> GetSet<T>() where T: class;
        Task SaveDbAsync();
    }
}
