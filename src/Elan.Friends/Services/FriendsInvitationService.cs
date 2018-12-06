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

        public async Task<FriendsInvitation> AcceptInvitation(ElanUser acceptingUser, ElanUser invitingUser)
        {
            var invitation = await _dataService
                .GetSet<FriendsInvitation>()
                .SingleAsync(i => i.UserFromId == invitingUser.Id && i.UserToId == acceptingUser.Id);

            invitation.IsAccepted = true;
            
            await _dataService.SaveDbAsync();

            return invitation;
        }

        public async Task DeclineInvitation(ElanUser decliningUser, ElanUser invitingUser)
        {
            var invitation = await _dataService
                .GetSet<FriendsInvitation>()
                .SingleAsync(i => i.UserFromId == invitingUser.Id && i.UserToId == decliningUser.Id);

            invitation.IsRejected = true;

            await _dataService.SaveDbAsync();
        }
        public async Task CancelInvitation(ElanUser cancelingUser, ElanUser invitedUser)
        {
            var invitation = await _dataService
                .GetSet<FriendsInvitation>()
                .SingleAsync(i => i.UserFromId == cancelingUser.Id && i.UserToId == invitedUser.Id);

            invitation.IsCanceled = true;

            await _dataService.SaveDbAsync();
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

        public async Task<bool> IsInvitedByUser(ElanUser invitedUser, ElanUser invitingUser)
        {
            FriendsInvitation invitation = null;

            invitation = await _dataService
                .GetSet<FriendsInvitation>()
                .Where(i => i.UserFromId == invitingUser.Id && i.UserToId == invitedUser.Id)
                .FirstOrDefaultAsync();

            if (invitation != null && !invitation.IsAccepted && !invitation.IsRejected && !invitation.IsCanceled)
            {
                return true;
            }

            return false;
        }
    }
}
