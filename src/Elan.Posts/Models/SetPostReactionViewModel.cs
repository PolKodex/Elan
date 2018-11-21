using Elan.Common.Enums;
using Elan.Data.Models.Account;

namespace Elan.Posts.Models
{
    public class SetPostReactionViewModel
    {
        public ElanUser User { get; set; }
        public int PostId { get; set; }
        public PostReactionType Type { get; set; }
    }
}
