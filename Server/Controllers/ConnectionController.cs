using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Server.Controllers.Interfaces;
using Server.Models;
using Sockets.DataStructures.Messages;
using Sockets.DataStructures.Services.Interfaces;

namespace Server.Controllers
{
    public class ConnectionController : IConnectionController
    {
        private readonly INetworkDataService _networkDataService;

        public ConnectionController(INetworkDataService networkDataService)
        {
                ClientConnections = new List<IClientConnection>();
            _networkDataService = networkDataService;
        }
        public List<IClientConnection> ClientConnections { get; private set;}

        public async Task<bool> TryAddUser(TcpClient newClient)
        {
            var stream = newClient.GetStream();

            var header = await _networkDataService.ReadAndDecodeHeader(stream);

            var message = await _networkDataService.ReadAndDecodeMessage(header, stream);

            if(message is NewUserOnlineMessage)
            {
                var newUserMessage = message as NewUserOnlineMessage;

                ClientConnections.Add(new ClientConnection(newUserMessage.UserId, stream));

                return true;
            }
            return false;
        }
    }
}