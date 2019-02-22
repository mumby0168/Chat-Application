using System;
using Server.Args;

namespace Server
{
    public interface IServerController
    {
         void Begin();

         EventHandler<NewClientEventArgs> UserConnected { get; set; }

         
    }
}