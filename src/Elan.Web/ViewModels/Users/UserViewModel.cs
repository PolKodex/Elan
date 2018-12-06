using Elan.Data.Models.Account;

namespace Elan.Web.ViewModels.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string AvatarBase64 { get; set; }

        public UserViewModel(ElanUser user)
        {
            Id = user.Id.ToString();
            UserName = user.GetDisplayName();
            AvatarBase64 = user.GetAvatar()?.RawValue;
        }
    }
}
