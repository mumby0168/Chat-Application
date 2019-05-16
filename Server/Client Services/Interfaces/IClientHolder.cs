using System.Collections.Generic;
using Server.Models;

namespace Server.Client_Services.Interfaces
{
    /// <summary>
    /// This holds all of the clients that have been conncted to the server. 
    /// </summary>
    public interface IClientsHolder
    {
        /// <summary>
        /// The collection of client connections.
        /// </summary>
         List<IClientConnection> ClientConnections {get;set;}
    }
}