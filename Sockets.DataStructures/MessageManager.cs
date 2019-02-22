using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures
{
    public class MessageRecEventArgs : EventArgs
    {
        public MessageRecEventArgs(IMessage message)
        {
            Message = message;
        }

        public IMessage Message { get; set; }   
    }
    public static class MessageManager
    {
        public static EventHandler<MessageRecEventArgs> MessageRecEventHandler;

        public static void Raise(object sender, IMessage message)
        {
            MessageRecEventHandler.Invoke(sender, new MessageRecEventArgs(message));
        }
    }

    
}
