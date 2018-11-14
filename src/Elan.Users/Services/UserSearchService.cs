using System;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Users.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Elan.Common.Contracts;

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

        public List<ElanUser> FindUsers(ElanUser user, string query)
        {
            if (!_queryValidationService.IsValidQuery(query))
            {
                throw new Exception($"Query is invalid: {query}");
            }

            var result = _dataService.GetSet<ElanUser>()
                .Where(x => x.UserName.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                .Include(x => x.FirstUserFriends)
                .Include(x => x.SecondUserFriends)
                .ToList();
                
            return result.ToList();
        }
    }
}
