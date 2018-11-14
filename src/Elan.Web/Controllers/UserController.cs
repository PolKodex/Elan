using System.Collections.Generic;
using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Account.Models;
using Elan.Users.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Elan.Web.Controllers
{
    public class UserController : ElanBaseController
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IUserService _userService;
        private readonly IUserSearchService _userSearchService;
        
        public UserController(
            IUserSettingsService userSettingsService,
            IUserService userService,
            IUserSearchService userSearchService)
        {
            _userSettingsService = userSettingsService;
            _userService = userService;
            _userSearchService = userSearchService;
        }

        [HttpGet]
        public async Task<List<UserSettingViewModel>> GetSettings()
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            var result = await _userSettingsService.GetSettingsForUser(currentUser);

            return result;
        }

        [HttpGet]
        public async Task<JsonResult> FindUsers(string query)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var foundUsers = _userSearchService.FindUsers(currentUser, query);

            // TODO: map users to whatever frontend needs

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
        public async Task ChangeSetting([FromBody]UserSettingViewModel setting)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            await _userSettingsService.ChangeSetting(currentUser, setting);
        }
    }
}
