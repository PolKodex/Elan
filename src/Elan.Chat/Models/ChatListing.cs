using System.Collections.Generic;
using Elan.Data.Models.Chat;

namespace Elan.Chat.Models
{
    public class ChatListing
    {
        public List<ChatMessage> Messages { get; set; }
        public int TotalCount { get; set; }
    }
}
