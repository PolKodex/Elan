using System.Threading.Tasks;
using Elan.Data.Models.Account;
using Elan.Data.Models.Chat;

namespace Elan.Chat.Contracts
{
    public interface IChatService
    {
        Task<ChatMessage> SaveMessage(ElanUser userFrom, ElanUser userTo, string message);
    }
}
