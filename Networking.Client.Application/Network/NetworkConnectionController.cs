using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.EventArgs;
using Networking.Client.Application.Network.Interfaces;
using Sockets.DataStructures.Messages;
using Sockets.DataStructures.Services.Interfaces;

namespace Networking.Client.Application.Network
{
    public class NetworkConnectionController : INetworkConnectionController
    {
        private readonly INetworkDataService _networkDataService;
        private Action _connectionFailedCallback;
        private Action _connectionSuccseful;

        public NetworkConnectionController(INetworkDataService networkDataService)
        {
            _networkDataService = networkDataService;            
        }
        private int _userId;
        private NetworkStream _stream;

        public void Connect(IPEndPoint ipEndPoint, int userId, Action connectedCallback, Action failedConnectionCallback)
        {
            _connectionFailedCallback = failedConnectionCallback;
            _connectionSuccseful = connectedCallback;
            _userId = userId;
            var tcpClient = new TcpClient();
            tcpClient.BeginConnect(ipEndPoint.Address, ipEndPoint.Port, ConnectionBegunCallback, tcpClient);
        }

        public EventHandler<MessageReceivedEventArgs> MessageReceivedEventHandler { get; set; }

        public void BeginListeningForMessages()
        {
            if (_stream == null)
                throw new InvalidOperationException("Connect() must be called prior to reading messages.");


            Task.Run(async () =>
            {
                while (true)
                {
                    if (_stream.DataAvailable)
                    {
                        Debug.WriteLine("Data being read.");
                        try
                        {
                            var header = await _networkDataService.ReadAndDecodeHeader(_stream);

                            var message = await _networkDataService.ReadAndDecodeMessage(header, _stream);

                            MessageReceivedEventHandler.Invoke(this, new MessageReceivedEventArgs{Message = message, TimeStamp = header.TimeStamp});
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                }
            });
        }      


        private async void ConnectionBegunCallback(IAsyncResult asyncResult)
        {
            try
            {
                var client = (TcpClient)asyncResult.AsyncState;                

                client.EndConnect(asyncResult);

                _stream = client.GetStream();

                if (_stream == null)
                {
                    _connectionFailedCallback.Invoke();
                    return;
                }

                var msg = new ConnectRequestMessage {ChatEnterMessage = "Hello", UserId = (ushort) _userId};

                await _networkDataService.WriteAndEncodeMessageWithHeader(msg, _stream);

                _connectionSuccseful.Invoke();
            }
            catch(Exception e)
            {
               if(_connectionFailedCallback == null) throw new Exception("No failed connection callback set.");
               _connectionFailedCallback.Invoke();            
            }
        }
    }
}
