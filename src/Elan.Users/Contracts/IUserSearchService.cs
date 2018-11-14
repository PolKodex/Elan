using System.Collections.Generic;
using Elan.Data.Models.Account;

namespace Elan.Users.Contracts
{
    public interface IUserSearchService
    {
        List<ElanUser> FindUsers(ElanUser user, string query);
    }
}
