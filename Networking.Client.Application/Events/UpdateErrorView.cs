using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.EventArgs;
using Prism.Events;

namespace Networking.Client.Application.Events
{
    public class UpdateErrorView : PubSubEvent<UpdateErrorViewEventArgs> {}
}
