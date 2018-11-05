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

        public async Task<FriendsRelation> CreateRelation(ElanUser FirstUser, ElanUser SecondUser)
        {
            var relation = new FriendsRelation
            {
                FirstUserId = FirstUser.Id,
                SecondUserId = SecondUser.Id,
                FirstUser = FirstUser,
                SecondUser = SecondUser,
                CreatedDate = DateTime.UtcNow
            };

            await _dataService.GetSet<FriendsRelation>().AddAsync(relation);
            await _dataService.SaveDbAsync();

            return relation;
        }


        public async Task<List<ElanUser>> GetFriendsForUser(ElanUser user)
        {
            var relations = await _dataService.GetSet<FriendsRelation>()
                .Include(u => u.FirstUser)
                .Include(u => u.SecondUser)
                .Where(u => u.FirstUser.Id == user.Id || u.SecondUser.Id == user.Id)
                .ToListAsync();

            var result = relations.Where(r => r.FirstUser.Id == user.Id).Select(r => r.SecondUser).ToList();
            result.AddRange(relations.Where(r => r.SecondUser.Id == user.Id).Select(r => r.FirstUser).ToList());

            return result;
        }
    }
}
