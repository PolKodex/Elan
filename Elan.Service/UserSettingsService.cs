using Elan.Data;
using Elan.Data.Models.Account;
using System;
using System.Threading.Tasks;
using Elan.Common.Dtos;
using Elan.Common.Enums;

namespace Elan.Service
{
    public interface IUserSettingsService
    {
        Task<UserSettingsDto> GetSettingsForUser(Guid userId);
        Task AddSettings(ElanUser newUser);
        Task ChangeSetting(Guid userId, UserSettingDto setting);
    }

    public class UserSettingsService : IUserSettingsService
    {
        private readonly IGenericRepository<ElanUserSetting> _repository;
        public UserSettingsService(IGenericRepository<ElanUserSetting> repository)
        {
            _repository = repository;
        }

        public async Task<UserSettingsDto> GetSettingsForUser(Guid userId)
        {
            var settings = await _repository.FindByAsync(x => x.UserId == userId);
            var result = new UserSettingsDto();
            settings.ForEach(x => result.AddSetting(x.Setting, x.PrivacySetting));
            return result;
        }

        public async Task AddSettings(ElanUser newUser)
        {
            var searchSetting = new ElanUserSetting()
            {
                UserId = newUser.Id,
                Setting = Setting.ShowInSearchResult,
                PrivacySetting = PrivacySetting.Everyone
            };
            _repository.Insert(searchSetting);

            var contentSetting = new ElanUserSetting()
            {
                UserId = newUser.Id,
                Setting = Setting.Content,
                PrivacySetting = PrivacySetting.Everyone
            };
            _repository.Insert(contentSetting);

            await _repository.SaveChangesAsync();
        }

        public async Task ChangeSetting(Guid userId, UserSettingDto setting)
        {
            var userSetting = new ElanUserSetting()
            {
                UserId = userId,
                Setting = setting.Setting,
                PrivacySetting = setting.PrivacySetting
            };
            _repository.Update(userSetting);
            await _repository.SaveChangesAsync();
        }
    }
}
