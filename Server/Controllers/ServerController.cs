using System.Net;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;
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

        public void Begin()
        {
            var hostName = Dns.GetHostName();

            var ipaddresses = Dns.GetHostAddresses(hostName);

            for(int i = 0 ; i < ipaddresses.Length; i++)
            {
                Console.WriteLine(i + ": " + ipaddresses[i]);
            }

            Console.WriteLine("Please select a ip address to run on: ");

            int input = int.Parse(Console.ReadLine());

            Console.WriteLine($"The IP Address selected is: {ipaddresses[input]}");


            TcpListener tcpListener;
            tcpListener = new TcpListener(ipaddresses[input], 2500);
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
            System.Console.WriteLine("Completed ending of connection.");

            var result = await _clientCreator.TryAddUser(newClient);

            if (!result)
            {
                newClient.Close();
                Console.WriteLine("Closing Connecition");
            }
        }
    }
}