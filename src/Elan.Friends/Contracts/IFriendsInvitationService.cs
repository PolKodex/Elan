using Elan.Data.Models.Account;
using Elan.Data.Models.Friends;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elan.Friends.Contracts
{
    public interface IFriendsInvitationService
    {
        Task<FriendsInvitation> CreateInvitation(ElanUser userFrom, ElanUser userTo);
        Task<List<FriendsInvitation>> GetPendingInvitationsForUser(ElanUser user);
        Task<bool> IsInvitedByUser(ElanUser invitedUser, ElanUser invitingUser);
        Task<FriendsInvitation> AcceptInvitation(ElanUser acceptingUser, ElanUser invitingUser);
        Task DeclineInvitation(ElanUser decliningUser, ElanUser invitingUser);
        Task CancelInvitation(ElanUser cancelingUser, ElanUser invitedUser);
    }
}
