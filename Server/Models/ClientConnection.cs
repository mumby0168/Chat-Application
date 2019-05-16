using System.Net.Sockets;
using Sockets.DataStructures.Services.Interfaces;

namespace Server.Models
{
    /// <summary>
    /// An interface to represent a client connection.
    /// </summary>
    public interface IClientConnection
    {
        int UserId { get; }
        NetworkStream Stream { get;}
    }
    public class ClientConnection : IClientConnection
    {
        public ClientConnection(int userId, NetworkStream stream)
        {
            UserId = userId;
            Stream = stream;
        }

        public int UserId { get; private set; }

        public NetworkStream Stream { get; private set; }
    }
}