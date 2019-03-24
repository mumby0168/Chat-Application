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

        public static ImageMessage Decode(byte[] encodedData)
        {
            var bytes = encodedData.ToList();
            var imageMessage = new ImageMessage();
            imageMessage.UserFromId = BitConverter.ToUInt16(bytes.GetRange(0, 2).ToArray(), 0);
            imageMessage.UserFromId = BitConverter.ToUInt16(bytes.GetRange(2, 2).ToArray(), 0);
            bytes.RemoveRange(0,4);
            imageMessage.ImageData = encodedData;
            return imageMessage;
        }
    }
}
