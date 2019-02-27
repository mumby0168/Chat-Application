using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.Models;
using Sockets.DataStructures.Base;

namespace Server.Client_Services.Interfaces
{
    public interface IClientWriter
    {
        Task WriteMessageToClient(int clientId, IMessage message);

        void WriteMessageToAllClients(IMessage message);        
    }
}