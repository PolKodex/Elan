using Elan.Data.Models.Account;

namespace Elan.Account.Models
{
    public class UserImageViewModel
    {
        public string Id { get; set; }
        public ElanUser User { get; set; }
        public string ImageBase64 { get; set; }
        public bool IsMain { get; set; }
    }
}
