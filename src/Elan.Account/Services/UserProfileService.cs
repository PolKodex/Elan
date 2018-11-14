using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Data.Contracts;

namespace Elan.Account.Services
{
    public class UserProfileService: IUserProfileService
    {
        private readonly IDataService _dataService;

        public UserProfileService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task UpdateProfile(UserProfileViewModel model)
        {
            model.User.Age = model.Age;
            model.User.Description = model.Description;
            model.User.FirstName = model.FirstName;
            model.User.LastName = model.LastName;

            await _dataService.SaveDbAsync();
        }
    }
}
