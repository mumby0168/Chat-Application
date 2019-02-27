

using Microsoft.Practices.Unity;
using Server.Client_Services;
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
            _unityContainer.RegisterSingleton<Client_Services.Interfaces.IClientsHolder, ClientsHolder>();
            _unityContainer.RegisterSingleton<Client_Services.Interfaces.IClientDisconnector, ClientDisconnector>();
            _unityContainer.RegisterSingleton<Client_Services.Interfaces.IClientWriter , ClientWriter>();
            _unityContainer.RegisterSingleton<Client_Services.Interfaces.IClientCreator, ClientCreator>();


            _unityContainer.RegisterType<INetworkDataService, NetworkDataService>();

        }
    }
}