using System;
using Sockets.DataStructures;

namespace Server
{
    class Program
    {
        private static IServerController _serverController;
        static void Main(string[] args)
        {
            _serverController = Bootstrapper.GetController();
            _serverController.Begin();
        }
    }
}
