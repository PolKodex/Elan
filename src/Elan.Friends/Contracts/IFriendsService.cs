using System.Collections.Generic;
using System.Threading.Tasks;
using Elan.Data.Models.Account;

namespace Elan.Friends.Contracts
{
    public interface IFriendsService
    {
        Task<List<ElanUser>> GetFriendsForUser(ElanUser user);
    }
}
