using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Client.Application.EventArgs
{
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
