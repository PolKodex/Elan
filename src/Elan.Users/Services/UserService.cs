using Elan.Data.Models.Account;
using Elan.Users.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Elan.Users.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<ElanUser> _userManager;

        public UserService(UserManager<ElanUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ElanUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task<ElanUser> GetUserByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }

        public async Task<ElanUser> GetUserByIdWithSettings(string id)
        {
            var user = await _userManager
                .Users
                .Include(x => x.Settings)
                .Include(x => x.FirstUserFriends)
                .Include(x => x.SecondUserFriends)
                .FirstOrDefaultAsync(x => x.Id.ToString() == id);
            return user;
        }

        public async Task<ElanUser> GetUserByNameWithSettings(string userName)
        {
            var user = await _userManager
                .Users
                .Include(x => x.Settings)
                .Include(x => x.FirstUserFriends)
                .Include(x => x.SecondUserFriends)
                .FirstOrDefaultAsync(x => x.UserName == userName);
            return user;
        }
    }
}
