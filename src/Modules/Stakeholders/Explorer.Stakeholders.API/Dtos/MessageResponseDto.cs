﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{

    public class MessageResponseDto
    {
        public long Id { get; set; }
        public long UserSenderId { get; set; }
        public long UserReciverId { get; set; }
        public string Text { get; set; }
        public string StatusOfMessage { get; set; }
    }
}
