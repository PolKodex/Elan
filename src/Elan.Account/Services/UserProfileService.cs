using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Microsoft.EntityFrameworkCore;

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
            var user = await _dataService.GetSet<ElanUser>().FirstOrDefaultAsync(x => x.Id.ToString() == model.Id);

            user.Age = model.Age;
            user.Description = model.Description;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await _dataService.SaveDbAsync();
        }
    }
}
