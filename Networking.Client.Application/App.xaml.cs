using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using CommonServiceLocator;
using Networking.Client.Application.Config;
using Networking.Client.Application.Network;
using Networking.Client.Application.Network.Interfaces;
using Networking.Client.Application.Services;
using Networking.Client.Application.Services.Concrete;
using Networking.Client.Application.ViewModels;
using Networking.Client.Application.Views;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using Sockets.DataStructures.Services;
using Sockets.DataStructures.Services.Interfaces;
using User.System.Core.Services;
using User.System.Core.Services.Interfaces;

namespace Networking.Client.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var classes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name.EndsWith("View"));

            foreach (var @class in classes)
            {
                containerRegistry.RegisterForNavigation(@class, @class.Name);
            }

            containerRegistry.Register<IPasswordProtectionService, PasswordProtectionService>();
            containerRegistry.Register<IFileProcessorService, FileProcessorService>();
            containerRegistry.RegisterSingleton<INetworkConnectionController, NetworkConnectionController>();
            containerRegistry.RegisterSingleton<ICurrentUser, CurrentUser>();
            containerRegistry.RegisterSingleton<IChatManager, ChatManager>();
            containerRegistry.Register<INetworkDataService, NetworkDataService>();
        }
    

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
        }


        protected override Window CreateShell()
        {
            return ServiceLocator.Current.GetInstance<MainWindow>();
        }
        
    }
}
