using Elan.Chat.Contracts;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Chat;
using System;
using System.Threading.Tasks;

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
    }
}
