using System;
using System.Collections.Generic;
using System.Text;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;

namespace Sockets.DataStructures.MockViewModels
{
    public class OneViewModel
    {
        public OneViewModel()
        {
            MessageManager.MessageRecEventHandler += (sender, args) =>
            {
                switch (args.Message.MessageType)
                {
                    case MessageType.NotSet:
                        break;
                    case MessageType.Connect:
                        ConnectRequestMessage = (ConnectRequestMessage) args.Message;
                        break;
                    case MessageType.Chat:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
        }

        public ConnectRequestMessage ConnectRequestMessage { get; set; }


     }
}

