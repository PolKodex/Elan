using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elan.Account.Models;
using Elan.Data.Models.Account;

namespace Elan.Account.Contracts
{
    public interface IUserSettingsService
    {
        Task<List<UserSettingViewModel>> GetSettingsForUser(ElanUser user);
        Task AddSettings(ElanUser newUser);
        Task ChangeSetting(ElanUser user, UserSettingViewModel setting);
        Dictionary<int, string> GetUserSettingsDict();
        Dictionary<int, string> GetPrivacySettingsDict();
    }
}
