using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models.MessagesViewModels
{
    public class ConversationsViewModel
    {
        public int SneakerId { get; set; }
        public string UserId { get; set; }
        public Sneaker Sneaker { get; set; }
        
    }
}
