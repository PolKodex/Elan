using System.Collections.Generic;
using System.Threading.Tasks;
using Elan.Data.Models.Account;
using Elan.Data.Models.Chat;

namespace Elan.Chat.Contracts
{
    public interface IChatService
    {
        Task<ChatMessage> SaveMessage(ElanUser userFrom, ElanUser userTo, string message);
        Task<List<ChatMessage>> GetMessagesAsync(ElanUser user1, ElanUser user2, int skip = 0, int take = 10);
    }
}
