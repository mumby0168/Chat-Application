using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Controllers.Interfaces
{
    /// <summary>
    /// A controller responsible for managing connections.
    /// </summary>
    public interface IConnectionController
    {   
        /// <summary>
        /// This begins reading from connected clients.
        /// </summary>
        void BeginReadingFromClients();
    }
}
