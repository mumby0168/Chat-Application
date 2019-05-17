using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.EventArgs;
using Prism.Events;

namespace Networking.Client.Application.Events
{
    /// <summary>
    /// An event that occurs when the error view should be updated.
    /// </summary>
    public class UpdateErrorView : PubSubEvent<UpdateErrorViewEventArgs> {}
}
