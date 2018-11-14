using Elan.Account.Models;

namespace Elan.Account.Contracts
{
    public interface IUserImageService
    {
        void UploadImage(UserImageViewModel model);
        void UpdateImage(UserImageViewModel model);
        void DeleteImage(string id);
    }
}
