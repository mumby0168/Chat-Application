using System;
using Sockets.DataStructures;

namespace Server
{
    class Program
    {
        private static IServerController _serverController;
        static void Main(string[] args)
        {
            Bootstrapper.Setup();
            _serverController = Bootstrapper.GetController();
            _serverController.Begin();

            Console.ReadLine();
        }
    }
}
