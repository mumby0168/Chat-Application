using System.Collections.Generic;
using Server.Models;

namespace Server.Extensions
{
    /// <summary>
    /// Extension methods to the list class.
    /// </summary>
    public static class ListIClientConnectionExtensions
    {
        /// <summary>
        /// Forces a copy of a list.
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        public static List<IClientConnection> TakeCopy(this List<IClientConnection> connections)
        {
            return new List<IClientConnection>(connections);
        }
    }
}