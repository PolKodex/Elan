using Elan.Account.Models;

namespace Elan.Account.Contracts
{
    public interface IAuthValidationService
    {
        void ValidateRegisterViewModel(RegisterViewModel model);
        void ValidateSignInViewModel(SignInViewModel model);
    }
}
