using System.Threading.Tasks;
using Elan.Account.Models;

namespace Elan.Account.Contracts
{
    public interface IAuthService
    {
        Task<string> Register(RegisterViewModel model);
        Task<string> SignIn(SignInViewModel model);
    }
}
