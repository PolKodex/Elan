using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Elan.Data.Contracts
{
    public interface IDataService
    {
        DbSet<T> GetSet<T>() where T: class;
        Task SaveDbAsync();
    }
}
