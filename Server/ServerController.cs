using System.Net;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;
using Server.Args;
using Server.Controllers;
using Server.Controllers.Interfaces;

namespace Server
{
    public class ServerController : IServerController
    {
        private readonly IConnectionController _connectionController;

        public ServerController(IConnectionController connectionController)
        {
            this._connectionController = connectionController;
        }

        public EventHandler<NewClientEventArgs> UserConnected { get; set; }

        public void Begin()
        {
            TcpListener tcpListener;
            tcpListener = new TcpListener(IPAddress.Parse("172.20.10.10"), 2500);
            tcpListener.Start();

            Task.Run(() => {
                while(true)
                {
                    if(tcpListener.Pending())
                    {
                        System.Console.WriteLine("new client requested");
                        tcpListener.BeginAcceptTcpClient(new AsyncCallback(NewConnectionCallback), tcpListener);
                    }
                }    
            });
        }

        private async void NewConnectionCallback(IAsyncResult asyncResult)
        {
            var listener = (TcpListener)asyncResult;

            var newClient = listener.EndAcceptTcpClient(asyncResult);

            var result = await _connectionController.TryAddUser(newClient);

            if(!result)
                newClient.Close();
        }
    }
}