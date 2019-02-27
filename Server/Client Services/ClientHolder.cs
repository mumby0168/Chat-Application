using System.Collections.Generic;
using Server.Client_Services.Interfaces;
using Server.Models;

namespace Server.Client_Services
{
    public class ClientsHolder : IClientsHolder
    {
        public ClientsHolder()
        {
            ClientConnections = new List<IClientConnection>();
        }

        public List<IClientConnection> ClientConnections { get; set; }
    }
}