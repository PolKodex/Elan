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
    public class FriendsInvitationService : IFriendsInvitationService
    {
        private readonly IDataService _dataService;

        public FriendsInvitationService(IDataService dataService)
        {
            _dataService = dataService;

        }

        public FriendsInvitation AcceptInvitation(ElanUser userFrom, ElanUser userTo)
        {
            var invitation = _dataService
                .GetSet<FriendsInvitation>()
                .Where(i => i.UserFromId == userFrom.Id && i.UserToId == userTo.Id)
                .Single();

            invitation.IsAccepted = true;

            _dataService.GetSet<FriendsInvitation>().Update(invitation);
            _dataService.SaveDbAsync();

            return invitation;
        }

        public async Task<FriendsInvitation> CreateInvitation(ElanUser userFrom, ElanUser userTo)
        {
            var invitation = new FriendsInvitation
            {
                UserFromId = userFrom.Id,
                UserToId = userTo.Id,
                CreatedDate = DateTime.UtcNow,
                IsAccepted = false
            };

            await _dataService.GetSet<FriendsInvitation>().AddAsync(invitation);
            await _dataService.SaveDbAsync();

            return invitation;
        }

        public async Task<List<FriendsInvitation>> GetNotAcceptedFriendsInvitationsForUser(ElanUser user)
        {
            var result = await _dataService.GetSet<FriendsInvitation>()
                .Include(i => i.UserFrom)
                .Where(i => i.UserToId == user.Id && i.IsAccepted == false)
                .ToListAsync();

            return result;
        }

        public async Task<bool> IsInvitedByUser(ElanUser currentUser, ElanUser targetUser)
        {
            FriendsInvitation invitation = null;

            try
            {
                invitation = await _dataService
                .GetSet<FriendsInvitation>()
                .Where(i => i.UserFromId == targetUser.Id && i.UserToId == currentUser.Id)
                .SingleAsync();
            }
            catch
            {
                //TODO - exception handling
            }

            if (invitation != null && !invitation.IsAccepted)
            {
                return true;
            }

            return false;
        }
    }
}
