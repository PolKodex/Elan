using System;
using Elan.Common.Enums;
using Elan.Data.Models.Account;

namespace Elan.Data.Models.Posts
{
    public class PostReaction
    {
        public int PostId { get; set; }
        public Guid UserId { get; set; }
        public PostReactionType Type { get; set; }
        public virtual Post Post { get; set; }
        public virtual ElanUser User { get; set; }
    }
}
