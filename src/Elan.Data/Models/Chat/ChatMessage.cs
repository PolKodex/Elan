using System;
using Elan.Data.Models.Account;

namespace Elan.Data.Models.Chat
{
    public class ChatMessage
    {
        public virtual ElanUser UserFrom { get; set; }
        public virtual ElanUser UserTo { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
    }
}
