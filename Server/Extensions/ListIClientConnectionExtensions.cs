using System.Collections.Generic;
using Server.Models;

namespace Server.Extensions
{
    public static class ListIClientConnectionExtensions
    {
        public static List<IClientConnection> TakeCopy(this List<IClientConnection> connections)
        {
            return new List<IClientConnection>(connections);
        }
    }
}