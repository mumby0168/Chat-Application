

using Microsoft.Practices.Unity;
using Server.Controllers;
using Server.Controllers.Interfaces;
using Server.Extensions;
using Sockets.DataStructures.Services;
using Sockets.DataStructures.Services.Interfaces;

namespace Server
{
    public static class Bootstrapper
    {
        private static IUnityContainer _unityContainer;
        

        public static IServerController GetController() => _unityContainer.Resolve<IServerController>();

        public static void Setup()
        {
            _unityContainer = new UnityContainer();
            
            _unityContainer.RegisterSingleton<IServerController, ServerController>();
            _unityContainer.RegisterSingleton<Controllers.Interfaces.IConnectionController, ConnectionController>();

            _unityContainer.RegisterType<INetworkDataService, NetworkDataService>();

        }
    }
}