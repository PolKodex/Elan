using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Friends.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elan.Friends.Services
{
    public class FriendsService: IFriendsService
    {
        private readonly IDataService _dataService;

        public FriendsService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<ElanUser>> GetFriendsForUser(ElanUser user)
        {
            // TODO: rework to fetch actual user friends
            var result = await _dataService.GetSet<ElanUser>()
                .Where(u => u.Id != user.Id)
                .ToListAsync();

            return result;
        }
    }
}
