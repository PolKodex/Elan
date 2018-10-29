using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Common.Exceptions;

namespace Elan.Account.Services
{
    public class AuthValidationService: IAuthValidationService
    {
        public void ValidateRegisterViewModel(RegisterViewModel model)
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

        public void ValidateSignInViewModel(SignInViewModel model)
        {
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
