using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Elan.Data.Models.Friends;

namespace Elan.Data.Models.Account
{
    public class ElanUser : IdentityUser<Guid>
    {
        public virtual ICollection<ElanUserSetting> Settings { get; set; }
        public virtual ICollection<FriendsRelation> FirstUserFriends { get; set; }
        public virtual ICollection<FriendsRelation> SecondUserFriends { get; set; }

        public ElanUser()
        {
            Settings = new List<ElanUserSetting>();
            FirstUserFriends = new List<FriendsRelation>();
            SecondUserFriends = new List<FriendsRelation>();
            SentFriendInvitations = new List<FriendsInvitation>();
            ReceivedFriendInvitations = new List<FriendsInvitation>();
        }

        [NotMapped]
        public virtual ICollection<FriendsRelation> Friends => FirstUserFriends.Union(SecondUserFriends).ToList();

        public virtual ICollection<FriendsInvitation> SentFriendInvitations { get; set; }
        public virtual ICollection<FriendsInvitation> ReceivedFriendInvitations { get; set; }
    }
}
