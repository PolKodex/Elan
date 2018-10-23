using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Common.Exceptions;
using Elan.Data.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;

namespace Elan.Account.Services
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<ElanUser> _userManager;

        public AuthService(UserManager<ElanUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Register(RegisterViewModel model)
        {
            ValidateRegistrationModel(model);

            var newUser = new ElanUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                throw new RegistrationFailedException(
                    $"An error occured while registering user: {result.Errors.Join()}");
            }
        }

        public Task SignIn(SignInViewModel model)
        {
            return Task.CompletedTask;
        }

        private void ValidateRegistrationModel(RegisterViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                throw new RegistrationFailedException(
                    $"Registration error: Email cannot be null!");
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                throw new RegistrationFailedException(
                    $"Registration error: Password cannot be null!");
            }

            if (string.IsNullOrEmpty(model.UserName))
            {
                throw new RegistrationFailedException(
                    $"Registration error: UserName cannot be null!");
            }
        }
    }
}
