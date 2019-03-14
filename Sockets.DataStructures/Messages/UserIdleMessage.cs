using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures.Messages
{
    public class UserIdleMessage : MessageBase
    {
        public UserIdleMessage()
        {
            MessageType = MessageType.Idle;
        }


        public ushort UserIdleId { get; set; }

        public override List<byte> Encode()
        {
            return BitConverter.GetBytes(UserIdleId).ToList();
        }

        public static UserIdleMessage Decode(List<byte> encodedBytes)
        {
            return new UserIdleMessage {UserIdleId = BitConverter.ToUInt16(encodedBytes.ToArray(), 0)};
        }
    }
}
