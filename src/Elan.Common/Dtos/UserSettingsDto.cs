using System.Collections.Generic;
using Elan.Common.Enums;
using Elan.Common.Extensions;

namespace Elan.Common.Dtos
{
    public class UserSettingsDto
    {
        public List<UserSettingDto> UserSettings { get; set; }
        public Dictionary<int, string> Settings => EnumHelper.GetEnumDictionary<Setting>();

        public Dictionary<int, string> PrivacySettings => EnumHelper.GetEnumDictionary<PrivacySetting>();

        public UserSettingsDto()
        {
            UserSettings = new List<UserSettingDto>();
        }

        public void AddSetting(Setting setting, PrivacySetting privacySetting)
        {
            UserSettings.Add(new UserSettingDto() { Setting = setting, PrivacySetting = privacySetting });
        }
    }
}