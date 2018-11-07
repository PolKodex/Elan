using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Friends;
using Elan.Friends.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elan.Friends.Services
{
    public class FriendsService : IFriendsService
    {
        private readonly IDataService _dataService;

        public FriendsService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<FriendsRelation> CreateRelation(ElanUser firstUser, ElanUser secondUser)
        {
            var relation = new FriendsRelation
            {
                FirstUserId = firstUser.Id,
                SecondUserId = secondUser.Id,
                FirstUser = firstUser,
                SecondUser = secondUser,
                CreatedDate = DateTime.UtcNow
            };

            await _dataService.GetSet<FriendsRelation>().AddAsync(relation);
            await _dataService.SaveDbAsync();

            return relation;
        }


        public async Task<List<ElanUser>> GetFriendsForUser(ElanUser user)
        {
            var result = await _dataService.GetSet<FriendsRelation>()
                .Include(u => u.FirstUser)
                .Include(u => u.SecondUser)
                .Where(u => u.FirstUser.Id == user.Id || u.SecondUser.Id == user.Id)
                .Select(r => GetFriendUser(r, user))
                .ToListAsync();
            return result;
        }

        private ElanUser GetFriendUser(FriendsRelation r, ElanUser user)
        {
            if (r.FirstUser.Id == user.Id)
            {
                return r.SecondUser;
            }
            return r.FirstUser;
        }
    }
}
