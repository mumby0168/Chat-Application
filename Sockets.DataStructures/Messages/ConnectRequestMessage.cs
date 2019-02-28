using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures.Messages
{
    public class ConnectRequestMessage : MessageBase
    {
        public ConnectRequestMessage()
        {       
            MessageType = MessageType.Connect;
        }
        
        public ushort UserId { get; set; }        

        public string ChatEnterMessage { get; set; }

        public static ConnectRequestMessage Decode(List<byte> messageData)
        {
            var message = new ConnectRequestMessage
            {
                UserId = BitConverter.ToUInt16(messageData.Take(2).ToArray(), 0)
            };
            messageData.RemoveRange(0, 2);
            message.ChatEnterMessage = Encoding.ASCII.GetString(messageData.ToArray());

            return message;
        }

        public override List<byte> Encode()
        {
            List<byte> data = new List<byte>();

            data.AddRange(BitConverter.GetBytes(UserId));
            data.AddRange(Encoding.ASCII.GetBytes(ChatEnterMessage));

            Size = (ushort) data.Count;

            return data;
        }
    }
}
