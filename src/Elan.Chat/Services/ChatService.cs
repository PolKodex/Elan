using Elan.Chat.Contracts;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Elan.Chat.Services
{
    public class ChatService: IChatService
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

        public async Task<List<ChatMessage>> GetMessagesAsync(ElanUser user1, ElanUser user2, int skip = 0, int take = 10)
        {
            var messages = 
                await _dataService
                    .GetSet<ChatMessage>()
                    .Include(m => m.UserFrom)
                    .Include(m => m.UserTo)
                    .Where(m => (m.UserFrom.Id == user1.Id && m.UserTo.Id == user2.Id)
                                || m.UserFrom.Id == user2.Id && m.UserTo.Id == user1.Id)
                    .OrderByDescending(m => m.SentOn)
                    .Skip(skip)
                    .Take(take)
                    .OrderBy(m => m.SentOn)
                    .ToListAsync();

            return messages;
        }
    }
}
