

using Microsoft.Practices.Unity;
using Server.Client_Services;
using Server.Controllers;
using Server.Controllers.Interfaces;
using Server.Extensions;
using Sockets.DataStructures.Services;
using Sockets.DataStructures.Services.Interfaces;

namespace Server
{
    /// <summary>
    /// Boostrapper to allow registration of types into DI container.
    /// </summary>
    public static class Bootstrapper
    {
        private static IUnityContainer _unityContainer;
        

        public static IServerController GetController() => _unityContainer.Resolve<IServerController>();

        /// <summary>
        /// Sets up the DI Container.
        /// </summary>
        public static void Setup()
        {
            _unityContainer = new UnityContainer();
            
            _unityContainer.RegisterSingleton<IServerController, ServerController>();
            _unityContainer.RegisterSingleton<Controllers.Interfaces.IConnectionController, ConnectionController>();
            _unityContainer.RegisterSingleton<Client_Services.Interfaces.IClientsHolder, ClientsHolder>();
            _unityContainer.RegisterSingleton<Client_Services.Interfaces.IClientDisconnector, ClientDisconnector>();
            _unityContainer.RegisterSingleton<Client_Services.Interfaces.IClientWriter , ClientWriter>();
            _unityContainer.RegisterSingleton<Client_Services.Interfaces.IClientCreator, ClientCreator>();


            _unityContainer.RegisterType<INetworkDataService, NetworkDataService>();

        }
    }
}