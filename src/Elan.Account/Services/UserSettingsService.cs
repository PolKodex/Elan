using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elan.Account.Contracts;
using Elan.Common.Enums;
using Elan.Data;
using Elan.Data.Models.Account;
using System.Linq;
using Elan.Account.Models;
using Elan.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Elan.Account.Services
{

    public class UserSettingsService : IUserSettingsService
    {
        private readonly ElanDbContext _db;

        public UserSettingsService(ElanDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserSettingViewModel>> GetSettingsForUser(Guid userId)
        {
            var userSettings = await _db.Set<ElanUserSetting>().Where(x => x.UserId == userId).ToListAsync();
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
            _db.Set<ElanUserSetting>().Add(searchSetting);

            var contentSetting = new ElanUserSetting()
            {
                UserId = newUser.Id,
                Setting = UserSetting.Content,
                PrivacySetting = PrivacySetting.Everyone
            };
            _db.Set<ElanUserSetting>().Add(contentSetting);

            await _db.SaveChangesAsync();
        }

        public async Task ChangeSetting(Guid userId, UserSettingViewModel setting)
        {
            var userSetting = new ElanUserSetting()
            {
                UserId = userId,
                Setting = setting.Setting,
                PrivacySetting = setting.PrivacySetting
            };
            _db.Update(userSetting);
            await _db.SaveChangesAsync();
        }
    }
}
