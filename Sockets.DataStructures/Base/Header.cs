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

        private const byte ConstantHeaderSize = 17;
    
        public Header(MessageType messageType, ulong messageSize)
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

        public UInt64 MessageSize { get; set; }



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

            MessageSize = BitConverter.ToUInt64(headerData.GetRange(1, 8).ToArray(), 0);

            TimeStamp = DateTime.FromBinary(BitConverter.ToInt64(headerData.GetRange(9, 8).ToArray(), 0));
        }
    }
}
