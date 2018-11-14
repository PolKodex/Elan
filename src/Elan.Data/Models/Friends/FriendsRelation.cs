using Elan.Data.Models.Account;
using System;

namespace Elan.Data.Models.Friends
{
    public class FriendsRelation
    {
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ElanUser FirstUser { get; set; }
        public virtual ElanUser SecondUser { get; set; }
    }
}
