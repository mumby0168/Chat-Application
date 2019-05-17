using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Networking.Client.Application.Events
{
    /// <summary>
    /// Event to be sent to indicate the base colour of the application changing.
    /// </summary>
    public class ChangeBaseColourEvent : PubSubEvent<bool>
    {
    }
}
