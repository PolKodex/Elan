using Elan.Data.Models.Friends;
using System;

namespace Elan.Web.ViewModels.Friends
{
    public class FriendsInvitationViewModel
    {
        public DateTime CreatedOn { get; set; }
        public string InvitationFrom { get; set; }
        public string InvitationFromId { get; set; }

        public FriendsInvitationViewModel(FriendsInvitation model)
        {
            CreatedOn = model.CreatedDate;
            InvitationFrom = model.UserFrom?.UserName;
            InvitationFromId = model.UserFrom.Id.ToString();
        }
    }
}
