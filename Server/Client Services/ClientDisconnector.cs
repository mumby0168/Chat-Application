using System;
using System.Linq;
using Server.Client_Services.Interfaces;
using Server.Controllers.Interfaces;
using Sockets.DataStructures.Messages;

namespace Server.Client_Services
{
    public class ClientDisconnector : IClientDisconnector
    {
        private readonly IClientsHolder _clientsHolder;
        private readonly IClientWriter _clientWriter;
        public ClientDisconnector(IClientsHolder clientsHolder, IClientWriter clientWriter)
        {
            this._clientWriter = clientWriter;
            this._clientsHolder = clientsHolder;

        }
        public void UserDisconnected(ushort userId)
        {
            var message = new UserOfflineMessage();
            message.UsersId = userId;

            RemoveUser(userId);

            _clientWriter.WriteMessageToAllClients(message);
        }

        private void RemoveUser(ushort userId)
        {
            var connection = _clientsHolder.ClientConnections.FirstOrDefault(c => c.UserId == userId);

            if (connection == null)
            {
                Console.WriteLine("Users being removed was not found in the connections list.");
                return;
            }

            connection.Stream.Close();
            _clientsHolder.ClientConnections.Remove(connection);
        }
    }
}