using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Sockets.DataStructures.Base
{
    /// <summary>
    /// BYTE | DATA
    ///  0   |  Message Type (ENUM)
    /// 
    ///  1   |  Size of Message |
    ///  2   |  Size of Message |
    /// 
    ///  3   |    Time Stamp    |
    ///  4   |                  |
    ///  5   |                  |
    ///  6   |                  |
    ///  7   |                  |
    ///  8   |                  |
    ///  9   |                  |
    ///  10  |                  |
    ///  11  |    Time Stamp    |
    /// </summary>
    public class Header
    {

        public static byte HeaderSize => ConstantHeaderSize;

        private List<byte> _encodedData;

        private const byte ConstantHeaderSize = 11;
    
        public Header(MessageType messageType, ushort messageSize)
        {
            MessageType = messageType;
            TimeStamp = DateTime.Now;
            MessageSize = messageSize;
            _encodedData = new List<byte> {Capacity = HeaderSize};
        }

        public Header(byte[] headerData)
        {
            Decode(headerData.ToList());
        }

        public Header(List<byte> headerData)
        {
            Decode(headerData);
        }

        public Header()
        {
            
        }


        public MessageType MessageType { get; set; }

        public DateTime TimeStamp { get; set; }

        public ushort MessageSize { get; set; }



        public List<byte> Encode()
        {
            _encodedData.Add((byte)MessageType);
            _encodedData.AddRange(BitConverter.GetBytes(MessageSize));
            _encodedData.AddRange(BitConverter.GetBytes(TimeStamp.ToBinary()));
            return _encodedData;
        }


        public void Decode(List<byte> headerData)
        {
            if(headerData.Count != HeaderSize) throw new ArgumentException("The header size must equal" + HeaderSize, "headerData");

            MessageType = (MessageType) headerData[0];

            MessageSize = BitConverter.ToUInt16(headerData.GetRange(1, 2).ToArray(), 0);

            TimeStamp = DateTime.FromBinary(BitConverter.ToInt64(headerData.GetRange(3, 8).ToArray(), 0));
        }
    }
}
