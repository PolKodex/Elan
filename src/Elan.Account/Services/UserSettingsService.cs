using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Common.Enums;
using Elan.Data.Models.Account;
using System.Linq;
using Elan.Account.Models;
using Elan.Common.Extensions;
using Elan.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Elan.Account.Services
{

    public class UserSettingsService : IUserSettingsService
    {
        private readonly IDataService _dataService;

        public UserSettingsService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<UserSettingViewModel>> GetSettingsForUser(ElanUser user)
        {
            var userSettings =
                await _dataService.GetSet<ElanUserSetting>().Where(x => x.UserId == user.Id).ToListAsync();

            var result = new List<UserSettingViewModel>();
            userSettings.ForEach(x =>
                result.Add(new UserSettingViewModel {Setting = x.Setting, PrivacySetting = x.PrivacySetting}));
            return result;
        }

        public Dictionary<int, string> GetUserSettingsDict()
        {
            return EnumHelper.GetEnumDictionary<UserSetting>();
        }
        public Dictionary<int, string> GetPrivacySettingsDict()
        {
            return EnumHelper.GetEnumDictionary<PrivacySetting>();
        }

        public async Task AddSettings(ElanUser newUser)
        {
            var searchSetting = new ElanUserSetting()
            {
                UserId = newUser.Id,
                Setting = UserSetting.ShowInSearchResult,
                PrivacySetting = PrivacySetting.Everyone
            };
            _dataService.GetSet<ElanUserSetting>().Add(searchSetting);

            var contentSetting = new ElanUserSetting()
            {
                UserId = newUser.Id,
                Setting = UserSetting.Content,
                PrivacySetting = PrivacySetting.Everyone
            };
            _dataService.GetSet<ElanUserSetting>().Add(contentSetting);

            await _dataService.SaveDbAsync();
        }

        public async Task ChangeSetting(ElanUser user, UserSettingViewModel setting)
        {
            var userSetting = new ElanUserSetting()
            {
                UserId = user.Id,
                Setting = setting.Setting,
                PrivacySetting = setting.PrivacySetting
            };
            _dataService.GetSet<ElanUserSetting>().Update(userSetting);
            await _dataService.SaveDbAsync();
        }
    }
}
