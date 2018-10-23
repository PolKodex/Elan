using System.Threading.Tasks;
using Elan.Account.Models;

namespace Elan.Account.Contracts
{
    public interface IAuthService
    {
        Task Register(RegisterViewModel model);
        Task SignIn(SignInViewModel model);
    }
}
