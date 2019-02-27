using System.Collections.Generic;
using Server.Models;

namespace Server.Client_Services.Interfaces
{
    public interface IClientsHolder
    {
         List<IClientConnection> ClientConnections {get;set;}
    }
}