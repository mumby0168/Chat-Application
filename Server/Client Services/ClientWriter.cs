using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Client_Services.Interfaces;
using Server.Controllers.Interfaces;
using Server.Extensions;
using Server.Models;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Services.Interfaces;

namespace Server.Client_Services
{
    public class ClientWriter : IClientWriter
    {
        private readonly IClientsHolder _clientsHolder;
        private readonly INetworkDataService _networkDataService;

        public ClientWriter(IClientsHolder clientHolder, INetworkDataService networkDataService)
        {
            this._networkDataService = networkDataService;
            this._clientsHolder = clientHolder;
        }
        public async void WriteMessageToAllClients(IMessage message)
        {
            var connections = _clientsHolder.ClientConnections.TakeCopy();

            foreach (var client in connections)
            {
                await Write(client, message);
            }
        }

        public async Task WriteMessageToClient(int clientId, IMessage message)
        {
            var client = _clientsHolder.ClientConnections.TakeCopy().FirstOrDefault(c => c.UserId == clientId);

            System.Console.WriteLine("Writing to message to client: " + clientId);

            await Write(client, message);
        }


        private async Task Write(IClientConnection clientConnection, IMessage message)
        {
            try
            {
                Console.WriteLine("Writing message type " + message.MessageType + " to user: " + clientConnection.UserId);
                await _networkDataService.WriteAndEncodeMessageWithHeader(message, clientConnection.Stream);
            }
            catch (Exception e)
            {
                _clientsHolder.ClientConnections.Remove(clientConnection);
                System.Console.WriteLine("Error in the Write() method + " + e.Message);
            }
        }
    }
}