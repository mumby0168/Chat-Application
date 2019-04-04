using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;
using Sockets.DataStructures.Services.Interfaces;

namespace Sockets.DataStructures.Services
{
    public class NetworkDataService : INetworkDataService
    {
        public async Task<Header> ReadAndDecodeHeader(NetworkStream networkStream)
        {
            byte[] headerBufferBytes = new byte[Header.HeaderSize];
            await networkStream.ReadAsync(headerBufferBytes, 0, headerBufferBytes.Length);
            var header = new Header(headerBufferBytes);
            return header;
        }

        public async Task<IMessage> ReadAndDecodeMessage(Header header, NetworkStream networkStream)
        {
            byte[] dataBuffer = new byte[header.MessageSize];

            await networkStream.ReadAsync(dataBuffer, 0, dataBuffer.Length);

            Console.WriteLine("message type being read in network service: " + header.MessageType);

            var dataBufferList = dataBuffer.ToList();

            switch (header.MessageType)
            {
                case MessageType.NotSet:
                    return null;
                case MessageType.Connect:
                    return ConnectRequestMessage.Decode(dataBufferList);
                case MessageType.Chat:
                    return ChatMessage.Decode(dataBufferList);
                case MessageType.Image:
                    return ImageMessage.Decode(dataBufferList);
                case MessageType.NewUserOnline:
                    return NewUserOnlineMessage.Decode(dataBufferList);
                case MessageType.UserLogoff:
                    return UserLogoffMessage.Decode(dataBufferList);
                case MessageType.UserOffline:
                    return UserOfflineMessage.Decode(dataBufferList);                    
                case MessageType.Typing:
                    return UserTypingMessage.Decode(dataBufferList);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task WriteAndEncodeMessageWithHeader(IMessage message, NetworkStream networkStream)
        {
            var messageData = message.Encode();
            var header = new Header(message.MessageType, (ushort) messageData.Count);
            var dataToWrite = new List<byte>();
            dataToWrite.AddRange(header.Encode());
            dataToWrite.AddRange(messageData);

            await networkStream.WriteAsync(dataToWrite.ToArray(), 0, dataToWrite.Count);            
        }
    }
}
