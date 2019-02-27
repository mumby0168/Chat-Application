using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Server.Client_Services.Interfaces;
using Server.Controllers.Interfaces;
using Server.Extensions;
using Server.Models;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;
using Sockets.DataStructures.Services.Interfaces;

namespace Server.Client_Services
{
    public class ClientCreator : IClientCreator
    {
        private readonly INetworkDataService _networkDataService;
        private readonly IClientWriter _clientWriter;
        private readonly IClientsHolder _clientsHolder;
        private readonly IClientDisconnector _clientDisconnector;

        public ClientCreator(INetworkDataService networkDataService, IClientWriter clientWriter, IClientsHolder clientsHolder, IClientDisconnector clientDisconnector)
        {
            this._clientDisconnector = clientDisconnector;
            this._clientsHolder = clientsHolder;
            this._clientWriter = clientWriter;
            this._networkDataService = networkDataService;

        }
        public async Task<bool> TryAddUser(TcpClient newClient)
        {
            var stream = newClient.GetStream();

            stream.ReadTimeout = 1000;

            IMessage message;
            try
            {
                var header = await _networkDataService.ReadAndDecodeHeader(stream);

                message = await _networkDataService.ReadAndDecodeMessage(header, stream);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }

            return await ProcessMessage(message, stream);
        }

        private async Task<bool> ProcessMessage(IMessage message, NetworkStream stream)
        {
            if (message is ConnectRequestMessage)
            {
                var connectRequestMessage = message as ConnectRequestMessage;

                var newMsg = new NewUserOnlineMessage() { UserId = connectRequestMessage.UserId };

                _clientWriter.WriteMessageToAllClients(newMsg);

                var existingConn = _clientsHolder.ClientConnections.FirstOrDefault(c => c.UserId == newMsg.UserId);

                if (existingConn != null)
                {
                    _clientDisconnector.UserDisconnected((ushort)existingConn.UserId);
                }

                var connection = new ClientConnection(connectRequestMessage.UserId, stream);

                await NotifyNewUserOfOtherConnections(connection);

                _clientsHolder.ClientConnections.Add(connection);

                System.Console.WriteLine("Client joined with ID: " + connectRequestMessage.UserId);

                return true;
            }
            return false;
        }

        private async Task NotifyNewUserOfOtherConnections(IClientConnection connection)
        {
            await Task.Run(async () =>
            {

                var connections = _clientsHolder.ClientConnections.TakeCopy();

                foreach (var conn in connections)
                {
                    Console.WriteLine("notfying new user: " + connection.UserId + " about existing user: " + conn.UserId);
                    var msg = new NewUserOnlineMessage { UserId = (ushort)conn.UserId };
                    await _networkDataService.WriteAndEncodeMessageWithHeader(msg, connection.Stream);
                }
            });
        }


    }
}