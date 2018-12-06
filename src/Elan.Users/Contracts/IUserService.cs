using System.Threading.Tasks;
using Elan.Data.Models.Account;

namespace Elan.Users.Contracts
{
    public interface IUserService
    {
        Task<ElanUser> GetUserById(string userId);
        Task<ElanUser> GetUserByName(string userName);
        Task<ElanUser> GetUserByNameWithSettings(string userName);
        Task<ElanUser> GetUserByIdWithSettings(string id);
    }
}
