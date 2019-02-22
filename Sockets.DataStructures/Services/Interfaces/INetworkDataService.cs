using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures.Services.Interfaces
{
    public interface INetworkDataService
    {
        Task<Header> ReadAndDecodeHeader(NetworkStream networkStream);

        Task<IMessage> ReadAndDecodeMessage(Header header, NetworkStream networkStream);

        Task WriteAndEncodeMessageWithHeader(IMessage message, NetworkStream networkStream);
    }
}
