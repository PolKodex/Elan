using System;
using System.Collections.Generic;
using Elan.Common.Enums;
using Elan.Data.Models.Account;

namespace Elan.Data.Models.Posts
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public PrivacySetting VisibilitySetting { get; set; }
        public virtual ElanUser CreatedBy { get; set; }
        public virtual ElanUser TargetUser { get; set; }
        public virtual ICollection<PostReaction> Reactions { get; set; }
    }
}
