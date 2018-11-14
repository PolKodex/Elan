using Elan.Data.Models.Account;
using System;

namespace Elan.Data.Models.Friends
{
    public class FriendsInvitation
    {
        public Guid UserToId { get; set; }
        public Guid UserFromId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ElanUser UserTo { get; set; }
        public virtual ElanUser UserFrom { get; set; }
        public bool IsAccepted { get; set; }
    }
}
