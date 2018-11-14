using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Data.Contracts;

namespace Elan.Account.Services
{
    public class UserImageService: IUserImageService
    {
        private readonly IDataService _dataService;

        public UserImageService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void UploadImage(UserImageViewModel model)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateImage(UserImageViewModel model)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteImage(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
