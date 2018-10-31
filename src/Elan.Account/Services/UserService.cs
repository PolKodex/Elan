using Elan.Account.Contracts;
using Elan.Data.Models.Account;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Elan.Account.Services
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
    }
}
