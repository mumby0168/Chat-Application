using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures.Messages
{
    public class UserTypingMessage : MessageBase
    {
        public UserTypingMessage()
        {
            MessageType = MessageType.Typing;
        }
        public ushort UserTypingId { get; set; }

        public ushort UserTypingToId { get; set; }

        public override List<byte> Encode()
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(UserTypingId));
            bytes.AddRange(BitConverter.GetBytes(UserTypingToId));
            return bytes;
        }

        public static UserTypingMessage Decode(List<byte> encodedBytes)
        {
            return new UserTypingMessage()
            {
                UserTypingId = BitConverter.ToUInt16(encodedBytes.GetRange(0, 2).ToArray(), 0),
                UserTypingToId = BitConverter.ToUInt16(encodedBytes.GetRange(2, 2).ToArray(), 0)
            };
        }
    }
}
