using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sockets.DataStructures.Messages
{
    public static class Extensions
    {
        public static byte[] GetBytes(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public static byte[] GetBytes(this ushort number)
        {
            return BitConverter.GetBytes(number);
        }

        public static ushort ToUInt16(this IEnumerable<byte> data)
        {
            return BitConverter.ToUInt16(data.ToArray(), 0);
        }

        public static string GetString(this IEnumerable<byte> data)
        {
            return Encoding.ASCII.GetString(data.ToArray());
        }

    }
}
