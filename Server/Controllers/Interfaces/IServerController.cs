using System;
using Server.Args;

namespace Server.Controllers.Interfaces
{
    public interface IServerController
    {
         void Begin();

         EventHandler<NewClientEventArgs> UserConnected { get; set; }         
    }
}