using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Common.Exceptions;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace Elan.Account.Services
{
    public class UserImageService : IUserImageService
    {
        private readonly IDataService _dataService;

        public UserImageService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<ElanUserImage> UploadImage(UserImageViewModel model)
        {
            var userImageSet = _dataService.GetSet<ElanUserImage>();

            var newUserImage = new ElanUserImage
            {
                IsMain = model.IsMain,
                RawValue = model.ImageBase64,
                User = model.User,
                UploadDate = DateTime.UtcNow
            };

            if (model.IsMain)
            {
                var currentMainImage = await userImageSet.FirstOrDefaultAsync(x => x.IsMain);
                if (currentMainImage != null)
                {
                    currentMainImage.IsMain = false;
                }
            }

            await userImageSet.AddAsync(newUserImage);

            await _dataService.SaveDbAsync();

            return newUserImage;
        }

        public async Task<ElanUserImage> UpdateImage(UserImageViewModel model)
        {
            var userImageSet = _dataService.GetSet<ElanUserImage>();

            var userImage = await userImageSet.FirstOrDefaultAsync(x => x.Id.ToString() == model.Id);

            if (userImage is null)
            {
                throw new InvalidUserImageIdException();
            }

            if (model.IsMain)
            {
                var currentMainImage = await userImageSet.FirstOrDefaultAsync(x => x.IsMain);
                if (currentMainImage != null)
                {
                    currentMainImage.IsMain = false;
                }
            }

            userImage.IsMain = model.IsMain;
            userImage.LastUpdateDate = DateTime.UtcNow;

            await _dataService.SaveDbAsync();

            return userImage;
        }

        public async Task<ElanUserImage> GetMainImage(ElanUser user)
        {
            var userImageSet = _dataService.GetSet<ElanUserImage>();

            var mainImage = await userImageSet.FirstOrDefaultAsync(x => x.UserId == user.Id && x.IsMain);

            return mainImage;
        }

        public async Task DeleteImage(string id)
        {
            var userImageSet = _dataService.GetSet<ElanUserImage>();

            var userImage = await userImageSet.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            if (userImage is null)
            {
                throw new InvalidUserImageIdException();
            }

            userImageSet.Remove(userImage);

            await _dataService.SaveDbAsync();
        }

        public async Task<IList<ElanUserImage>> GetUserImages(ElanUser user)
        {
            var userImageSet = _dataService.GetSet<ElanUserImage>();

            var userImages = 
                await userImageSet
                    .Where(ui => ui.UserId == user.Id)
                    .OrderByDescending(ui => ui.UploadDate)
                    .ToListAsync();

            return userImages;
        }
    }
}
