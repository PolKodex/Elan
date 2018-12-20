using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Common.Enums;
using Elan.Friends.Contracts;
using Elan.Users.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elan.Web.Controllers
{
    // TODO: access rights need to be checked before each action executed
    // TODO: custom controller method attributes: Owner [user is owner of the entity - like in UserProfile], Participant [user is part of the entity - like in FriendsRelationRequest]
    [Authorize]
    public class UserController : ElanBaseController
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IUserService _userService;
        private readonly IUserSearchService _userSearchService;
        private readonly IUserProfileService _userProfileService;
        private readonly IUserImageService _userImageService;
        private readonly IFriendsInvitationService _friendsInvitationService;

        public UserController(
            IUserSettingsService userSettingsService,
            IUserService userService,
            IUserSearchService userSearchService,
            IUserProfileService userProfileService,
            IUserImageService userImageService, 
            IFriendsInvitationService friendsInvitationService)
        {
            _userSettingsService = userSettingsService;
            _userService = userService;
            _userSearchService = userSearchService;
            _userProfileService = userProfileService;
            _userImageService = userImageService;
            _friendsInvitationService = friendsInvitationService;
        }

        [HttpGet]
        public async Task<List<UserSettingViewModel>> GetSettings()
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            var result = await _userSettingsService.GetSettingsForUser(currentUser);

            return result;
        }

        [HttpGet]
        public async Task<JsonResult> FindUsers(string query, int skip = 0, int take = 10)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var foundUsers = _userSearchService.FindUsers(currentUser, query, skip, take);

            return Json(foundUsers);
        }

        [HttpGet]
        public IActionResult GetUserSettingsDict()
        {
            return Ok(_userSettingsService.GetUserSettingsDict());
        }

        [HttpGet]
        public IActionResult GetPrivacySettingsDict()
        {
            return Ok(_userSettingsService.GetPrivacySettingsDict());
        }

        [HttpPut]
        public async Task ChangeSettings([FromBody]List<UserSettingViewModel> settings)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            await _userSettingsService.ChangeSettings(currentUser, settings);
        }
        
        [HttpPut]
        public async Task<ViewModels.Users.UserProfileViewModel> UpdateProfile([FromBody]UserProfileViewModel model)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            if (!currentUser.Id.ToString().Equals(model.Id))
            {
                throw new InvalidOperationException("You can't edit other user profile info.");
            }
            
            var user = await _userProfileService.UpdateProfile(model);
            var mainImage = await _userImageService.GetMainImage(user);

            var result = new ViewModels.Users.UserProfileViewModel(user);

            if (mainImage != null)
            {
                result.MainImage = new ViewModels.Users.UserImageViewModel(mainImage);
            }

            return result;
        }

        [HttpGet]
        public async Task<ViewModels.Users.UserProfileViewModel> GetUserProfile(string userId)
        {
            var user = await _userService.GetUserByIdWithSettings(userId);
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var result = new ViewModels.Users.UserProfileViewModel(user);

            result.IsFriend =
                user.Friends.Any(x => x.FirstUserId == currentUser.Id || x.SecondUserId == currentUser.Id);

            var userPrivacySetting = user.Settings.FirstOrDefault(x => x.Setting == UserSetting.ProfileVisibility);
            if (userPrivacySetting != null && userId != currentUser.Id.ToString())
            {
                if (userPrivacySetting.PrivacySetting == PrivacySetting.Friends)
                {
                    if (!result.IsFriend)
                    {
                        result.IsPrivate = true;
                    }
                }
            }

            if (!result.IsFriend)
            {
                result.InvitedByMe = await _friendsInvitationService.IsInvitedByUser(user, currentUser);

                if (!result.InvitedByMe)
                {
                    result.InvitedMe = await _friendsInvitationService.IsInvitedByUser(currentUser, user);
                }
            }
          
            var mainImage = await _userImageService.GetMainImage(user);
      
            if (mainImage != null)
            {
                result.MainImage = new ViewModels.Users.UserImageViewModel(mainImage);
            }

            return result;
        }

        [HttpPost]
        public async Task<ViewModels.Users.UserImageViewModel> UploadImage([FromBody]UserImageViewModel model)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            model.User = currentUser;

            var result = await _userImageService.UploadImage(model);

            return new ViewModels.Users.UserImageViewModel(result);
        }

        [HttpPut]
        public async Task<ViewModels.Users.UserImageViewModel> UpdateImage([FromBody]UserImageViewModel model)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            model.User = currentUser;

            var result = await _userImageService.UploadImage(model);

            return new ViewModels.Users.UserImageViewModel(result);
        }

        [HttpDelete]
        public async Task DeleteImage(string imageId)
        {
            await _userImageService.DeleteImage(imageId);
        }

        [HttpGet]
        public async Task<IEnumerable<ViewModels.Users.UserImageViewModel>> GetUserImages(string userId)
        {
            var user = await _userService.GetUserById(userId);

            var userImages = await _userImageService.GetUserImages(user);

            return userImages.Select(x => new ViewModels.Users.UserImageViewModel(x));
        }
    }
}
