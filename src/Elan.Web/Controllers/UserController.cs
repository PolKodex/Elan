using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Account.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elan.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserSettingsService _userSettingsService;

        public UserController(IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;
        }

        [HttpGet]
        public async Task<List<UserSettingViewModel>> GetSettings()
        {
            var userId = new Guid("104D8337-1565-4F2C-A8E3-08D639BC745F");
            var result = await _userSettingsService.GetSettingsForUser(userId);
            return result;
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
            var userId = new Guid("104D8337-1565-4F2C-A8E3-08D639BC745F");
            await _userSettingsService.ChangeSetting(userId, setting);
        }
    }
}
