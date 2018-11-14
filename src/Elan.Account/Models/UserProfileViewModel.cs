using Elan.Data.Models.Account;

namespace Elan.Account.Models
{
    public class UserProfileViewModel
    {
        public ElanUser User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string Description { get; set; }
    }
}
