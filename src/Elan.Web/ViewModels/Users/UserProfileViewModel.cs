using Elan.Data.Models.Account;

namespace Elan.Web.ViewModels.Users
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public int? Age { get; set; }
        public UserImageViewModel MainImage { get; set; }

        public UserProfileViewModel(ElanUser user)
        {
            Id = user.Id.ToString();
            FirstName = user.FirstName;
            LastName = user.LastName;
            Description = user.Description;
            Age = user.Age;
        }
    }
}
