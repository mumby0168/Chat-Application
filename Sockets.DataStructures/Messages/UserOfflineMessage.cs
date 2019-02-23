using System.Linq;
using System;
using System.Collections.Generic;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures.Messages
{
    public class UserOfflineMessage : MessageBase
    {
        public UserOfflineMessage()
        {
            MessageType = MessageType.UserOffline;
        }

        public ushort UsersId { get; set; }

        public override List<byte> Encode() 
        {
            return BitConverter.GetBytes(UsersId).ToList();
        }

        public static UserOfflineMessage Decode(List<byte> encodedData)
        {
            return new UserOfflineMessage{UsersId = BitConverter.ToUInt16(encodedData.ToArray(), 0)};
        }
    }
}