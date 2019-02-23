using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Client.Application.Services.Concrete
{
    public class CurrentUser : ICurrentUser
    {
        public int Id { get; set; }
        public NetworkStream NetworkStream { get; set; }        
    }
}
