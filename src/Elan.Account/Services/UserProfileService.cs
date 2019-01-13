using System.IO;
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

        public async Task<ElanUser> UpdateProfile(UserProfileViewModel model)
        {
            if (model.Age < 13)
            {
                throw new InvalidDataException("Age cannot be lower than 13.");
            }

            var user = await _dataService.GetSet<ElanUser>().FirstOrDefaultAsync(x => x.Id.ToString() == model.Id);

            user.Age = model.Age;
            user.Description = model.Description;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            if (user.Gender != 0 && user.Gender != model.Gender)
            {
                throw new InvalidDataException("Gender cannot be changed.");
            }
            if (user.Gender == 0)
            {
                user.Gender = model.Gender;
            }
            
            await _dataService.SaveDbAsync();
            return user;
        }
    }
}
