using System.Net;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;
using Server.Args;
using Server.Controllers;
using Server.Controllers.Interfaces;
using IConnectionController = Server.Controllers.Interfaces.IConnectionController;
using Server.Extensions;
using Sockets.DataStructures.Messages;
using Server.Client_Services.Interfaces;

namespace Server.Controllers
{
    public class ServerController : IServerController
    {
        private readonly IConnectionController _connectionController;
        private readonly IClientCreator _clientCreator;

        public ServerController(IConnectionController connectionController, IClientCreator clientCreator)
        {
            this._clientCreator = clientCreator;
            this._connectionController = connectionController;
        }

        public EventHandler<NewClientEventArgs> UserConnected { get; set; }

        public void Begin()
        {
            var hostName = Dns.GetHostName();

            var ipaddresses = Dns.GetHostAddresses(hostName);

            Console.WriteLine("Server running on: " + ipaddresses[1]);

            TcpListener tcpListener;
            tcpListener = new TcpListener(ipaddresses[1], 2500);
            tcpListener.Start();

            _connectionController.BeginReadingFromClients();

            Task.Run(() =>
            {
                while (true)
                {
                    if (tcpListener.Pending())
                    {
                        System.Console.WriteLine("new client requested");
                        tcpListener.BeginAcceptTcpClient(new AsyncCallback(NewConnectionCallback), tcpListener);
                    }
                }
            });
        }

        private async void NewConnectionCallback(IAsyncResult asyncResult)
        {
            var listener = (TcpListener)asyncResult.AsyncState;

            var newClient = listener.EndAcceptTcpClient(asyncResult);
            System.Console.WriteLine("Completed ending of connectio.");

            var result = await _clientCreator.TryAddUser(newClient);

            if (!result)
            {
                newClient.Close();
                System.Console.WriteLine("Closing Connecition");
            }
        }
    }
}