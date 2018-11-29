using System.Threading.Tasks;
using Elan.Account.Models;
using Elan.Data.Models.Account;

namespace Elan.Account.Contracts
{
    public interface IUserProfileService
    {
        Task<ElanUser> UpdateProfile(UserProfileViewModel model);
    }
}
