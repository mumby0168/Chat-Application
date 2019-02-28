using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using User.System.Core.Model;

namespace Networking.Client.Application.Events
{
    public class EditUserEvent : PubSubEvent<SocketUser>
    {
    }
}
