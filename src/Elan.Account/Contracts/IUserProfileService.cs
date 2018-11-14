using System.Threading.Tasks;
using Elan.Account.Models;

namespace Elan.Account.Contracts
{
    public interface IUserProfileService
    {
        Task UpdateProfile(UserProfileViewModel model);
    }
}
