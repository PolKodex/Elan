using System;
using System.Threading.Tasks;
using Elan.Common.Dtos;
using Elan.Service;
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
        public async Task<UserSettingsDto> GetSettings()
        {
            var userId = new Guid();
            var result = await _userSettingsService.GetSettingsForUser(userId);
            return result;
        }

        [HttpPut]
        public async Task ChangeSetting([FromBody]UserSettingDto setting)
        {
            var userId = new Guid();
            await _userSettingsService.ChangeSetting(userId, setting);
        }
    }
}
