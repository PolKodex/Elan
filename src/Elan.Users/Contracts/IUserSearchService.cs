using System.Collections.Generic;
using Elan.Data.Models.Account;
using Elan.Users.Models;

namespace Elan.Users.Contracts
{
    public interface IUserSearchService
    {
        List<UserSearchResultViewModel> FindUsers(ElanUser user, string query, int skip, int take);
    }
}
