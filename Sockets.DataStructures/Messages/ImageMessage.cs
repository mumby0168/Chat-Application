using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures.Messages
{
    public class ImageMessage : MessageBase
    {
        public ImageMessage()
        {
            MessageType = MessageType.Image;
        }


        public ushort UserFromId { get; set; }

        public ushort UserToId { get; set; }

        public byte[] ImageData { get; set; }

        public override List<byte> Encode()
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(UserFromId));
            bytes.AddRange(BitConverter.GetBytes(UserToId));
            bytes.AddRange(ImageData);
            return bytes;
        }

        public static ImageMessage Decode(List<byte> encodedData)
        {            
            var imageMessage = new ImageMessage();
            imageMessage.UserFromId = BitConverter.ToUInt16(encodedData.GetRange(0, 2).ToArray(), 0);
            imageMessage.UserToId = BitConverter.ToUInt16(encodedData.GetRange(2, 2).ToArray(), 0);
            encodedData.RemoveRange(0,4);
            imageMessage.ImageData = encodedData.ToArray();
            return imageMessage;
        }
    }
}
