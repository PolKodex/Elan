﻿using Elan.Data.Models.Account;
using Elan.Users.Contracts;
using Microsoft.AspNetCore.Identity;
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
    }
}