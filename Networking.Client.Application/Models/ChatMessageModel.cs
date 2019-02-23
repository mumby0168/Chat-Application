using System;

namespace Networking.Client.Application.Models
{
    public class ChatMessageModel
    {         

        public string Message { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool IsSent { get; set; }

    }
}