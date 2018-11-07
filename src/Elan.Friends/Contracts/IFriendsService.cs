using Elan.Data.Models.Account;
using Elan.Data.Models.Friends;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elan.Friends.Contracts
{
    public interface IFriendsService
    {
        Task<FriendsRelation> CreateRelation(ElanUser firstUser, ElanUser secondUser);
        Task<List<ElanUser>> GetFriendsForUser(ElanUser user);
    }
}
