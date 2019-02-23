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

            stream.ReadTimeout = 1000;

            IMessage message;

            try{
                var header = await _networkDataService.ReadAndDecodeHeader(stream);

                message = await _networkDataService.ReadAndDecodeMessage(header, stream);
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }

            if(message is ConnectRequestMessage)
            {
                var connectRequestMessage = message as ConnectRequestMessage;

                var newMsg = new NewUserOnlineMessage(){UserId = connectRequestMessage.UserId };

                WriteMessageToAllClients(newMsg);

                var connection = new ClientConnection(connectRequestMessage.UserId, stream);

                await NotifyNewUserOfOtherConnections(connection);

                ClientConnections.Add(connection);

                System.Console.WriteLine("Client joined with ID: " + connectRequestMessage.UserId);

                return true;
            }
            return false;
        }

        private async Task NotifyNewUserOfOtherConnections(IClientConnection connection)
        {
            await Task.Run(async () => {

                var connections = ClientConnections.TakeCopy();

                foreach(var conn in connections)
                {
                    Console.WriteLine("notfying new user: " + connection.UserId + " about existing user: " + conn.UserId);
                    var msg = new NewUserOnlineMessage{UserId = (ushort)conn.UserId};
                    await _networkDataService.WriteAndEncodeMessageWithHeader(msg, connection.Stream);
                }
            });
        }

        private void UserDisconnected(ushort userId)
        {
            var message = new UserOfflineMessage();
            message.UsersId = userId;

            RemoveUser(userId);

            WriteMessageToAllClients(message);
        }

        public void BeginReadingFromClients()
        {
            Task.Run(async () => {
                while(true)
                {
                    var clients = ClientConnections.TakeCopy();

                    foreach(var client in clients)
                    {
                        try
                        {
                            if(client.Stream.DataAvailable)
                            {
                                var header = await _networkDataService.ReadAndDecodeHeader(client.Stream);

                                var message = await _networkDataService.ReadAndDecodeMessage(header, client.Stream);

                                await ProcessMessage(message);
                            }
                        }
                        catch (System.Exception e)
                        {
                            Console.WriteLine("Begin reading exception: " + e.Message);
                        }
                    }
                }
            });
        }

        private async Task WriteMessageToClient(int clientId, IMessage message)
        {
            var client = ClientConnections.TakeCopy().FirstOrDefault(c => c.UserId == clientId);

            System.Console.WriteLine("Writing to message to client: " + clientId);

            await Write(client, message);
        }

        private async void WriteMessageToAllClients(IMessage message)
        {
            var connections = ClientConnections.TakeCopy();

            foreach(var client in connections)
            {
                await Write(client, message);
            }
        }

        private async Task Write(IClientConnection clientConnection, IMessage message)
        {
                try
                {
                    await _networkDataService.WriteAndEncodeMessageWithHeader(message, clientConnection.Stream);
                }
                catch(Exception e)
                {
                    System.Console.WriteLine("Exception thrown in Write() " + e.Message);
                    System.Console.WriteLine("client with id " + clientConnection.UserId + " Disconnected.");
                    clientConnection.Stream.Close();
                    ClientConnections.Remove(clientConnection);
                }
        }

        private async Task ProcessMessage(IMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.Chat:
                    var newMsg = message as ChatMessage;
                    System.Console.WriteLine("Chat message read.");
                    await WriteMessageToClient(newMsg.UserToId, newMsg);
                    break;
                case MessageType.UserLogoff:
                    var msg = message as UserLogoffMessage;
                    RemoveUser(msg.UsersId);
                    WriteMessageToAllClients(new UserOfflineMessage{UsersId = msg.UsersId});
                    break;
            }
        }

        private void RemoveUser(ushort userId)
        {
                    var connection = ClientConnections.FirstOrDefault(c => c.UserId == userId);

                    if(connection == null)
                    {
                        Console.WriteLine("Users being removed was not found in the connections list.")
                        return;
                    }

                    connection.Stream.Close();
                    ClientConnections.Remove(connection);
        }
    }
}