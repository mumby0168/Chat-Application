using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Sockets.DataStructures.Base;

namespace Sockets.DataStructures.Messages
{
    class ImageMessage : MessageBase
    {
        public ImageMessage()
        {
            MessageType = MessageType.Image;
        }


        public ushort FromId { get; set; }

        public ushort ToId { get; set; }

        public byte[] ImageData { get; set; }

        public override List<byte> Encode()
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(FromId));
            bytes.AddRange(BitConverter.GetBytes(ToId));
            bytes.AddRange(ImageData);
        }

        public static ImageMessage Decode(byte[] encodedData)
        {
            var bytes = encodedData.ToList();
            var imageMessage = new ImageMessage();
            imageMessage.FromId = BitConverter.ToUInt16(bytes.GetRange(0, 2).ToArray(), 0);
            imageMessage.FromId = BitConverter.ToUInt16(bytes.GetRange(2, 2).ToArray(), 0);
            bytes.RemoveRange(0,4);
            imageMessage.ImageData = encodedData;
            return imageMessage;
        }
    }
}
