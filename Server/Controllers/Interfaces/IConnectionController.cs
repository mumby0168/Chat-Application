using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Controllers.Interfaces
{
    public interface IConnectionController
    {
        Task<bool> TryAddUser(TcpClient newClient);

        List<IClientConnection> ClientConnections { get; }
    }
}
