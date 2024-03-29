﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models.MessagesViewModels
{
    public class MessagesCreateViewModel
    {
        public string SneakerOwnerId { get; set; }
        public string BuyerId { get; set; }
        public Message Message { get; set; }
        public Sneaker Sneaker { get; set; }
    }
}
