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
            tcpListener = new TcpListener(IPAddress.Parse("192.168.1.97"), 2500);
            tcpListener.Start();

            _connectionController.BeginReadingFromClients();

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
           
            var listener = (TcpListener)asyncResult.AsyncState;

            var newClient = listener.EndAcceptTcpClient(asyncResult);
            System.Console.WriteLine("Completed ending of connectio.");

            var result = await _connectionController.TryAddUser(newClient);

            if(!result){
                newClient.Close();
                System.Console.WriteLine("Closing Connecition");
            }
        }
    }
}