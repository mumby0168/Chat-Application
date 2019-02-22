using System;
using System.Collections.Generic;
using System.Text;

namespace Sockets.DataStructures.Base
{
    public interface IMessage
    {
        ushort Size { get; }
        
        List<byte> Encode();

        MessageType MessageType { get; }
    }
}
