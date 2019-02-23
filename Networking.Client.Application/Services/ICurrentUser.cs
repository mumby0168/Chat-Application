using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Client.Application.Services
{
    public interface ICurrentUser
    {
        int Id { get; set; }

        NetworkStream NetworkStream { get; set; }
    }
}
