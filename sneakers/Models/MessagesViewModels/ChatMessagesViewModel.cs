using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models.MessagesViewModels
{
    public class ChatMessagesViewModel
    {
        public List<Message> ChatMessages { get; set; }
        public string MsgChat { get; set; }
        public int SneakerId { get; set; }
        public Message Message { get; set; }
        public string OtherUserId { get; set; }
    }
}
