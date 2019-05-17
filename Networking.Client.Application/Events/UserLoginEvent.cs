using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using User.System.Core.Model;

namespace Networking.Client.Application.Events
{
    /// <summary>
    /// An event that occurs when a user logs in.
    /// </summary>
    public class UserLoginEvent : PubSubEvent<SocketUser>
    {
    }
}
