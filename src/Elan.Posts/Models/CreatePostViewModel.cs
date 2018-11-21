using Elan.Common.Enums;

namespace Elan.Posts.Models
{
    public class PostViewModel
    {
        public int? PostId { get; set; }
        public string Content { get; set; }
        public string ToUserId { get; set; } = null;
        public int? BasePostId { get; set; } = null;
        public PrivacySetting? PrivacySetting { get; set; } = null;
    }
}
