﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using IMessage = Sockets.DataStructures.Base.IMessage;

namespace Networking.Client.Application.EventArgs
{
    /// <summary>
    /// Event arguments to be sent when a message is received by the client.
    /// </summary>
    public class MessageReceivedEventArgs : System.EventArgs
    {
    
        public IMessage Message { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}   
