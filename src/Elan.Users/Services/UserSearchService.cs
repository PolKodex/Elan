using Elan.Common.Contracts;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Users.Contracts;
using Elan.Users.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                            x.FirstName.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                            !string.IsNullOrWhiteSpace(x.LastName) && x.LastName.Contains(query,
                                StringComparison.CurrentCultureIgnoreCase))
                .OrderByDescending(x => x.FirstName.Contains(query, StringComparison.CurrentCultureIgnoreCase) &&
                                        x.LastName.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(x => x.LastName.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(x => x.FirstUserFriends.Any(y =>
                                           y.SecondUser.FirstUserFriends.Any(z => z.SecondUserId == user.Id) ||
                                           y.SecondUser.SecondUserFriends.Any(z => z.FirstUserId == user.Id))
                                       ||
                                       x.SecondUserFriends.Any(y =>
                                           y.FirstUser.FirstUserFriends.Any(z => z.SecondUserId == user.Id) ||
                                           y.FirstUser.SecondUserFriends.Any(z => z.FirstUserId == user.Id)))
                .Skip(skip * take)
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
                        : string.Empty,
                    Id = x.Id.ToString()
                })
                .ToList();


            foreach (var searchResult in result.Where(x => !string.IsNullOrEmpty(x.ImageRawValue)))
            {
                int commaIndex = searchResult.ImageRawValue.IndexOf(',', StringComparison.Ordinal);
                var imagePrefix = searchResult.ImageRawValue.Substring(0, commaIndex+1);
                var imageBase = searchResult.ImageRawValue.Substring(commaIndex+1, searchResult.ImageRawValue.Length-commaIndex-1);
                byte[] imageBytes = Convert.FromBase64String(imageBase);
                IImageFormat format;
                using (var image = Image.Load(imageBytes, out format))
                {
                    image.Mutate(x => x
                        .Resize(30, 30));

                    using (var output = new MemoryStream())
                    {
                        image.Save(output, format);
                        var resized = output.ToArray();
                        searchResult.ImageRawValue = imagePrefix + Convert.ToBase64String(resized);
                    }
                }
                
            }

            return result;
        }

    }
}
