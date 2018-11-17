using Elan.Data.Models.Friends;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Elan.Data.Models.Posts;

namespace Elan.Data.Models.Account
{
    public class ElanUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public int? Age { get; set; }
        public virtual ICollection<ElanUserImage> Images { get; set; }
        public virtual ICollection<Post> PostedByUser { get; set; }
        public virtual ICollection<Post> PostedToUser { get; set; }
        public virtual ICollection<ElanUserSetting> Settings { get; set; }
        public virtual ICollection<FriendsRelation> FirstUserFriends { get; set; }
        public virtual ICollection<FriendsRelation> SecondUserFriends { get; set; }
        [NotMapped]
        public virtual ICollection<FriendsRelation> Friends => FirstUserFriends.Union(SecondUserFriends).ToList();

        public ElanUser()
        {
            Settings = new List<ElanUserSetting>();
            FirstUserFriends = new List<FriendsRelation>();
            SecondUserFriends = new List<FriendsRelation>();
            PostedByUser = new List<Post>();
            PostedToUser = new List<Post>();
            Images = new List<ElanUserImage>();
        }
        
        public bool IsFriend(ElanUser user)
        {
            return FirstUserFriends.Any(x => x.FirstUserId == user.Id || x.SecondUserId == user.Id) || SecondUserFriends.Any(x => x.FirstUserId == user.Id || x.SecondUserId == user.Id);
        }

//        public bool IsConnected(ElanUser currentUser)
//        {
//            return Friends.Any(x => x.FirstUser.Friends.Any(y=>y.FirstUser.IsFriend(currentUser) || y.FirstUser.IsFriend(currentUser)) == user.Id || x.SecondUserId == user.Id);
//        }
    }
}
