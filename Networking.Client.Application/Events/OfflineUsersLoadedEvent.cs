using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Networking.Client.Application.Events
{
    /// <summary>
    /// An event that occurs when a new users data has been loaded from the database.
    /// </summary>
    public class OfflineUsersLoadedEvent : PubSubEvent
    {
    }
}
