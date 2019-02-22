using System;
using System.Collections.Generic;
using System.Text;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;

namespace Sockets.DataStructures.MockViewModels
{
    public class TwoViewModel
    {      
            public TwoViewModel()
            {
                MessageManager.MessageRecEventHandler += (sender, args) =>
                {
                    switch (args.Message.MessageType)
                    {
                        case MessageType.NotSet:
                            break;
                        case MessageType.Connect:
                            break;
                        case MessageType.Chat:
                            ChatMessage = (ChatMessage) args.Message;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                };
            }

            public ChatMessage ChatMessage { get; set; }
    
    }
}
