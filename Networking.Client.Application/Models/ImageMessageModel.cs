using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Client.Application.Models
{
    /// <summary>
    /// A model to encapsulate the data required for a image message.
    /// </summary>
    public class ImageMessageModel
    {
        public bool IsSent { get; set; }

        public DateTime TimeStamp { get; set; }

        public byte[] ImageData { get; set; }
    }
}
