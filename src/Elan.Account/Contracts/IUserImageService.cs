using System.Collections.Generic;
using System.Threading.Tasks;
using Elan.Account.Models;
using Elan.Data.Models.Account;

namespace Elan.Account.Contracts
{
    public interface IUserImageService
    {
        Task<ElanUserImage> UploadImage(UserImageViewModel model);
        Task<ElanUserImage> UpdateImage(UserImageViewModel model);
        Task<ElanUserImage> GetMainImage(ElanUser user);
        Task DeleteImage(string id);
        Task<IList<ElanUserImage>> GetUserImages(ElanUser user);
    }
}
