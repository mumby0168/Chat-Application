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
    /// An event that occurs when changes are made a users profile.
    /// </summary>
    class UserEditedEvent : PubSubEvent<SocketUser>
    {
    }
}
