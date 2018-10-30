using System.Threading.Tasks;
using Elan.Data.Models.Account;

namespace Elan.Account.Contracts
{
    public interface IUserService
    {
        Task<ElanUser> GetUser(string userId);
    }
}
