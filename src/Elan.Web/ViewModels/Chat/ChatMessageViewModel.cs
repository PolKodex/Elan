using System;
using Elan.Data.Models.Chat;

namespace Elan.Web.ViewModels.Chat
{
    public class ChatMessageViewModel
    {
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }

        public ChatMessageViewModel(ChatMessage model)
        {
            FromUserId = model.UserFrom.Id.ToString();
            ToUserId = model.UserTo.Id.ToString();
            Content = model.Content;
            SentOn = model.SentOn;
        }
    }
}
