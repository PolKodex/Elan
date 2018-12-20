using Elan.Chat.Contracts;
using Elan.Chat.Models;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Chat;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Elan.Chat.Services
{
    public class ChatService : IChatService
    {
        private readonly IDataService _dataService;

        public ChatService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<ChatMessage> SaveMessage(ElanUser userFrom, ElanUser userTo, string message)
        {
            var chatMessage = new ChatMessage
            {
                UserFrom = userFrom,
                UserTo = userTo,
                Content = message,
                SentOn = DateTime.UtcNow
            };

            await _dataService.GetSet<ChatMessage>().AddAsync(chatMessage);
            await _dataService.SaveDbAsync();

            return chatMessage;
        }

        public async Task<ChatListing> GetMessagesAsync(ElanUser user1, ElanUser user2, int skip = 0, int take = 10)
        {
            var messagesQuery = _dataService
                .GetSet<ChatMessage>()
                .Where(m => (m.UserFromId == user1.Id && m.UserToId == user2.Id) ||
                            (m.UserFromId == user2.Id && m.UserToId == user1.Id));

            var totalCount = messagesQuery.Count();

            var messages =
                await messagesQuery
                    .OrderByDescending(m => m.SentOn)
                    .Skip(skip * take)
                    .Take(take)
                    .OrderBy(m => m.SentOn)
                    .ToListAsync();

            var model = new ChatListing
            {
                TotalCount = totalCount,
                Messages = messages
            };

            return model;
        }
    }
}
