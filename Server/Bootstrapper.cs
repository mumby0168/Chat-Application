

using Microsoft.Practices.Unity;
using Server.Controllers;
using Server.Controllers.Interfaces;
using Server.Extensions;
using Sockets.DataStructures.Services;
using Sockets.DataStructures.Services.Interfaces;

namespace Server
{
    public class Bootstrapper
    {
        private static IUnityContainer _unityContainer;
        public Bootstrapper()
        {
            _unityContainer = new UnityContainer();
            Setup();
        }
           
        public static IServerController GetController() => _unityContainer.Resolve<IServerController>();

        private static void Setup()
        {
            _unityContainer.RegisterSingleton<IServerController, ServerController>();
            _unityContainer.RegisterSingleton<IConnectionController, ConnectionController>();

            _unityContainer.RegisterType<INetworkDataService, NetworkDataService>();

        }
    }
}