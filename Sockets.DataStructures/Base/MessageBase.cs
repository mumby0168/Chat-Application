using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace Sockets.DataStructures.Base
{
    public abstract class MessageBase : IMessage
    {

        protected MessageBase()
        {
            MessageType = MessageType.NotSet;
        }             

        public  MessageType MessageType { get; protected set; }

        public ushort Size { get; protected set; }        

        public abstract List<byte> Encode();

        public virtual Header GetHeader()
        {
            return new Header(MessageType, CalculateSizeMessageSize());
        }

        protected virtual ushort CalculateSizeMessageSize()
        {
            return (ushort) Encode().Count;
        } 

        public virtual List<byte> GetMessageAndHeaderBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(GetHeader().Encode());
            bytes.AddRange(Encode());

            return bytes;
        }


    }
}
