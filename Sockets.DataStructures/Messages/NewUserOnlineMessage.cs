using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures.Messages
{
    public class NewUserOnlineMessage : MessageBase
    {

        public NewUserOnlineMessage()
        {
            MessageType = MessageType.NewUserOnline;
        }

        public ushort UserId { get; set; }

        public override List<byte> Encode()
        {
            return BitConverter.GetBytes(UserId).ToList();
        }

        public static NewUserOnlineMessage Decode(List<byte> encodedData)
        {
            if(encodedData.Count != 2) throw new ArgumentOutOfRangeException("The message size must be 2 bytes.");

            return new NewUserOnlineMessage{ UserId = BitConverter.ToUInt16(encodedData.ToArray(),0) };
        }
    }
}
