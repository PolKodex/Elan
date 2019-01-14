using Elan.Common.Enums;

namespace Elan.Account.Models
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public Gender Gender { get; set; }
    }
}
