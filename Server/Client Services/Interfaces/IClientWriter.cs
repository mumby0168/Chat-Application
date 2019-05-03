using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.Models;
using Sockets.DataStructures.Base;

namespace Server.Client_Services.Interfaces
{
    /// <summary>
    /// This service is responsible for writing messages to clients.
    /// </summary>
    public interface IClientWriter
    {
       /// <summary>
       /// Writes a message to a specified client.
       /// </summary>
       /// <param name="clientId"></param>
       /// <param name="message"></param>
       /// <returns></returns>
        Task WriteMessageToClient(int clientId, IMessage message);

       /// <summary>
       /// Write a message to all clients connected.
       /// </summary>
       /// <param name="message"></param>
        void WriteMessageToAllClients(IMessage message);        
    }
}