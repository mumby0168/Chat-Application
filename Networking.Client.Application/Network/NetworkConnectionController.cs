using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Networking.Client.Application.EventArgs;
using Networking.Client.Application.Network.Interfaces;
using Networking.Client.Application.Services;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;
using Sockets.DataStructures.Services.Interfaces;

namespace Networking.Client.Application.Network
{
    public class NetworkConnectionController : INetworkConnectionController
    {
        private readonly INetworkDataService _networkDataService;
        private readonly ICurrentUser _currentUser;
        private Action _connectionFailedCallback;
        private Action _connectionSuccseful;

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        public NetworkConnectionController(INetworkDataService networkDataService, ICurrentUser currentUser)
        {
            _networkDataService = networkDataService;
            _currentUser = currentUser;
            _cancellationTokenSource = new CancellationTokenSource();
           
        }        

        public void Connect(IPEndPoint ipEndPoint, int userId, Action connectedCallback, Action failedConnectionCallback)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _connectionFailedCallback = failedConnectionCallback;
            _connectionSuccseful = connectedCallback;
            _currentUser.Id = userId;
            var tcpClient = new TcpClient();
            tcpClient.BeginConnect(ipEndPoint.Address, ipEndPoint.Port, ConnectionBegunCallback, tcpClient);
        }

        public EventHandler<MessageReceivedEventArgs> MessageReceivedEventHandler { get; set; }

        public void Disconnect()
        {
            _cancellationTokenSource.Cancel();            
        }

        public void BeginListeningForMessages()
        {
            if (_currentUser.NetworkStream == null)
                throw new InvalidOperationException("Connect() must be called prior to reading messages.");


            Task.Run(async () =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    if (_currentUser.NetworkStream.DataAvailable)
                    {
                        Debug.WriteLine("Data being read.");
                        try
                        {
                            var header = await _networkDataService.ReadAndDecodeHeader(_currentUser.NetworkStream);

                            var message = await _networkDataService.ReadAndDecodeMessage(header, _currentUser.NetworkStream);

                            if(message.MessageType == MessageType.UserOffline) Console.WriteLine("User Offline message recveid ......");

                            MessageReceivedEventHandler.Invoke(this, new MessageReceivedEventArgs{Message = message, TimeStamp = header.TimeStamp});
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }

                _currentUser.NetworkStream.Close();

            }, _cancellationToken);
        }

        public async Task SendMessage(IMessage message)
        {
           await _networkDataService.WriteAndEncodeMessageWithHeader(message, _currentUser.NetworkStream);
        }


        private async void ConnectionBegunCallback(IAsyncResult asyncResult)
        {
            try
            {
                var client = (TcpClient)asyncResult.AsyncState;                

                client.EndConnect(asyncResult);

                _currentUser.NetworkStream = client.GetStream();

                if (_currentUser.NetworkStream == null)
                {
                    _connectionFailedCallback.Invoke();
                    return;
                }

                var msg = new ConnectRequestMessage {ChatEnterMessage = "Hello", UserId = (ushort)_currentUser.Id};

                await _networkDataService.WriteAndEncodeMessageWithHeader(msg, _currentUser.NetworkStream);

                _connectionSuccseful.Invoke();
            }
            catch(Exception)
            {
               if(_connectionFailedCallback == null) throw new Exception("No failed connection callback set.");
               _connectionFailedCallback.Invoke();            
            }
        }
    }
}
