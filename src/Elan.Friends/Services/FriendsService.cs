using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Friends;
using Elan.Friends.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elan.Common.Exceptions;

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
                .Include(u => u.FirstUser.Images)
                .Include(u => u.SecondUser.Images)
                .Where(u => u.FirstUser.Id == user.Id || u.SecondUser.Id == user.Id)
                .Select(r => GetFriendUser(r, user))
                .ToListAsync();

            return result;
        }

        public async Task RemoveRelation(ElanUser currentUser, ElanUser user)
        {
            var friendsRelationsSet = _dataService.GetSet<FriendsRelation>();

            var relation = await friendsRelationsSet.FirstOrDefaultAsync(x =>
                (x.FirstUserId == currentUser.Id && x.SecondUserId == user.Id) ||
                (x.SecondUserId == currentUser.Id && x.FirstUserId == user.Id));

            if (relation == null)
            {
                throw new RelationNotFoundException();
            }

            friendsRelationsSet.Remove(relation);

            await _dataService.SaveDbAsync();
        }

        private ElanUser GetFriendUser(FriendsRelation relation, ElanUser user)
        {
            if (relation.FirstUser.Id == user.Id)
            {
                return relation.SecondUser;
            }

            return relation.FirstUser;
        }
    }
}
