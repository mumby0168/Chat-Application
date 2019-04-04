using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Client.Application.Models
{
    public class ImageMessageModel
    {
        public bool IsSent { get; set; }

        public DateTime TimeStamp { get; set; }

        public byte[] ImageData { get; set; }
    }
}
