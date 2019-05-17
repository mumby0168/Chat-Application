using System;

namespace Networking.Client.Application.Models
{
    /// <summary>
    /// A model to encapsulate the data for a chat message.
    /// </summary>
    public class ChatMessageModel
    {
        
        public string Message { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool IsSent { get; set; }

    }
}