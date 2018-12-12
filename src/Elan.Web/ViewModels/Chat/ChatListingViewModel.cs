using System.Collections.Generic;
using System.Linq;
using Elan.Chat.Models;

namespace Elan.Web.ViewModels.Chat
{
    public class ChatListingViewModel
    {
        public List<ChatMessageViewModel> Messages { get; set; }
        public int TotalCount { get; set; }

        public ChatListingViewModel(ChatListing model, string currentUserId = "")
        {
            Messages = model.Messages.Select(x => new ChatMessageViewModel(x, currentUserId)).ToList();
            TotalCount = model.TotalCount;
        }
    }
}
