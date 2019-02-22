using System.Collections.Generic;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures.Messages
{
    public class ChatMessage : MessageBase  
    {
        public ChatMessage()
        {
            MessageType = MessageType.Chat;
        }

        public ushort UserToId { get; set; }

        public ushort UserFromId { get; set; }

        public string Message { get; set; }      

        public static ChatMessage Decode(List<byte> messageData)
        {
            var message = new ChatMessage();
            message.UserFromId = messageData
                .GetRange(0, 2)
                .ToUInt16();

            message.UserToId = messageData
                .GetRange(2, 2)
                .ToUInt16();

            messageData.RemoveRange(0, 4);

            message.Message = messageData.GetString();


            return message;
        }

        public override List<byte> Encode()
        {
            var data = new List<byte>();            

            data.AddRange(UserFromId.GetBytes());

            data.AddRange(UserToId.GetBytes());     

            data.AddRange(Message.GetBytes());            

            return data;
        }
    }
}
