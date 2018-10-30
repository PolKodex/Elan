using Elan.Chat.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Chat;
using System;

namespace Elan.Chat.Services
{
    public class ChatService: IChatService
    {
        public ChatMessage SaveMessage(ElanUser userFrom, ElanUser userTo, string message)
        {
            throw new NotImplementedException();
        }
    }
}
