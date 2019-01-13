using Elan.Common.Enums;

namespace Elan.Account.Models
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string Description { get; set; }
        public Gender Gender { get; set; }
    }
}
