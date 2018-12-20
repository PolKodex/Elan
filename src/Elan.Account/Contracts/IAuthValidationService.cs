using System.Threading.Tasks;
using Elan.Account.Models;

namespace Elan.Account.Contracts
{
    public interface IAuthValidationService
    {
        Task ValidateRegisterViewModel(RegisterViewModel model);
        Task ValidateSignInViewModel(SignInViewModel model);
        Task ValidatePasswordHintQuestion(string userName);
        Task ValidateChangePasswordViewModel(ChangePasswordViewModel model);
    }
}
