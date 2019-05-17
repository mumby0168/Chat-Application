using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Networking.Client.Application.Events
{
    /// <summary>
    /// An event to be sent when the user logs off.
    /// </summary>
    public class LogoffEvent : PubSubEvent
    {
    }
}
