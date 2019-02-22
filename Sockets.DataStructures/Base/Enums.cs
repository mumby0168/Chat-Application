using System;
using System.Collections.Generic;
using System.Text;

namespace Sockets.DataStructures.Base
{
    public enum MessageType : byte
    {
        NotSet = 0,
        Connect = 1,
        Chat = 2,
        Image = 3,
        NewUserOnline = 4
    }

}
