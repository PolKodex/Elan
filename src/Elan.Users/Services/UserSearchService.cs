using System;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Users.Contracts;
using System.Collections.Generic;
using System.Linq;
using Elan.Account.Models;
using Elan.Common.Contracts;
using Elan.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Elan.Users.Services
{
    public class UserSearchService: IUserSearchService
    {
        private readonly IDataService _dataService;
        private readonly IQueryValidationService _queryValidationService;

        public UserSearchService(IDataService dataService, IQueryValidationService queryValidationService)
        {
            _dataService = dataService;
            _queryValidationService = queryValidationService;
        }

        public List<UserSearchResultViewModel> FindUsers(ElanUser user, string query, int skip, int take)
        {
            if (!_queryValidationService.IsValidQuery(query)) throw new Exception($"Query is invalid: {query}");

            var result = _dataService.GetSet<ElanUser>()
                .Where(x => !string.IsNullOrWhiteSpace(x.FirstName) &&
                            query.Contains(x.FirstName, StringComparison.CurrentCultureIgnoreCase) ||
                            !string.IsNullOrWhiteSpace(x.LastName) && query.Contains(x.LastName,
                                StringComparison.CurrentCultureIgnoreCase))
                .OrderByDescending(x => query.Contains(x.FirstName, StringComparison.CurrentCultureIgnoreCase) &&
                                        query.Contains(x.LastName, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(x => query.Contains(x.LastName, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(x => x.FirstUserFriends.Any(y =>
                                           y.SecondUser.FirstUserFriends.Any(z => z.SecondUserId == user.Id) ||
                                           y.SecondUser.SecondUserFriends.Any(z => z.FirstUserId == user.Id))
                                       ||
                                       x.SecondUserFriends.Any(y =>
                                           y.FirstUser.FirstUserFriends.Any(z => z.SecondUserId == user.Id) ||
                                           y.FirstUser.SecondUserFriends.Any(z => z.FirstUserId == user.Id)))
                .Skip(skip)
                .Take(take)
                .Select(x => new UserSearchResultViewModel
                {
                    MutualFriendsCount = x.FirstUserFriends.Count(y =>
                                             y.SecondUser.FirstUserFriends.Any(z => z.SecondUserId == user.Id) ||
                                             y.SecondUser.SecondUserFriends.Any(z => z.FirstUserId == user.Id))
                                         +
                                         x.SecondUserFriends.Count(y =>
                                             y.FirstUser.FirstUserFriends.Any(z => z.SecondUserId == user.Id) ||
                                             y.FirstUser.SecondUserFriends.Any(z => z.FirstUserId == user.Id)),
                    FirstName = x.FirstName, LastName = x.LastName,
                    ImageRawValue = x.Images.FirstOrDefault(y => y.IsMain) != null
                        ? x.Images.FirstOrDefault(y => y.IsMain).RawValue
                        : string.Empty
                })
                .ToList();

            return result;
        }

    }
}
