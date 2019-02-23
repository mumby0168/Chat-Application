using System;
using System.Collections.Generic;
using System.Linq;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures.Messages
{
    public class UserLogoffMessage : MessageBase
    {
        public UserLogoffMessage()
        {
            MessageType = MessageType.UserLogoff;   
        }

       public ushort UsersId { get; set; }

        public override List<byte> Encode() 
        {
            return BitConverter.GetBytes(UsersId).ToList();
        }

        public static UserLogoffMessage Decode(List<byte> encodedData)
        {
            return new UserLogoffMessage{UsersId = BitConverter.ToUInt16(encodedData.ToArray(), 0)};
        }
    }
}