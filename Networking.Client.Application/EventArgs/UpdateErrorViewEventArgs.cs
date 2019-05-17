using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Client.Application.EventArgs
{
    /// <summary>
    /// Event arguments to be sent when an update to the error window needs to be made.
    /// </summary>
    public class UpdateErrorViewEventArgs
    {
        public UpdateErrorViewEventArgs(string title, List<string> messages = null)
        {
            Messages = messages;
            Title = title;
        }

        public string Title { get; set; }

        public List<string> Messages { get; set; }
    }
}
