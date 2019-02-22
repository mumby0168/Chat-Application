using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.EventArgs;

namespace Networking.Client.Application.Network.Interfaces
{
    public interface INetworkConnectionController
    {
        void Connect(IPEndPoint ipEndPoint, int userId, Action connectedCallback, Action failedConnectionCallback);
        
        EventHandler<MessageReceivedEventArgs> MessageReceivedEventHandler { get; set; }

        void BeginListeningForMessages();        
    }
}
