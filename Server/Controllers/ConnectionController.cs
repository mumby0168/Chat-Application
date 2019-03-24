using System.Net.Http.Headers;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Server.Controllers.Interfaces;
using Server.Models;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;
using Sockets.DataStructures.Services.Interfaces;
using Server.Extensions;
using Server.Client_Services.Interfaces;

namespace Server.Controllers
{
    public class ConnectionController : IConnectionController
    {
        private readonly INetworkDataService _networkDataService;
        private readonly IClientsHolder _clientsHolder;
        private readonly IClientWriter _clientWriter;
        private readonly IClientDisconnector _clientDisconnector;

        public ConnectionController(INetworkDataService networkDataService, IClientsHolder clientsHolder, IClientWriter clientWriter, IClientDisconnector clientDisconnector)
        {
            this._clientDisconnector = clientDisconnector;
            this._clientWriter = clientWriter;
            this._clientsHolder = clientsHolder;
            _networkDataService = networkDataService;
        }

        public void BeginReadingFromClients()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var clients = _clientsHolder.ClientConnections.TakeCopy();

                    foreach (var client in clients)
                    {
                        try
                        {
                            if (client.Stream.DataAvailable)
                            {
                                var header = await _networkDataService.ReadAndDecodeHeader(client.Stream);

                                var message = await _networkDataService.ReadAndDecodeMessage(header, client.Stream);

                                await ProcessMessage(message);
                            }
                        }
                        catch (System.Exception e)
                        {
                            Console.WriteLine("Begin reading exception: " + e.Message + " on client with id: " + client.UserId);
                        }
                    }
                }
            });
        }

        private async Task ProcessMessage(IMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.Chat:
                    var newMsg = message as ChatMessage;
                    System.Console.WriteLine("Chat message read.");
                    await _clientWriter.WriteMessageToClient(newMsg.UserToId, newMsg);
                    break;
                case MessageType.UserLogoff:
                    var msg = message as UserLogoffMessage;
                    _clientDisconnector.UserDisconnected(msg.UsersId);
                    _clientWriter.WriteMessageToAllClients(new UserOfflineMessage { UsersId = msg.UsersId });
                    break;
                case MessageType.Typing:
                    var typingMessage = message as UserTypingMessage;
                    await _clientWriter.WriteMessageToClient(typingMessage.UserTypingToId, message);
                    break;     
                case MessageType.Image:
                    var imageMessage = message as ImageMessage;
                    await _clientWriter.WriteMessageToClient(imageMessage.UserToId, message);
                    break;
            }
        }



    }
}