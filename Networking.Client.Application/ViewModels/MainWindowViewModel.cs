using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.Config;
using Networking.Client.Application.Views;
using Prism.Mvvm;
using Prism.Regions;

namespace Networking.Client.Application.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(LoginView));
            _regionManager.RegisterViewWithRegion(RegionNames.LeftPanel, typeof(UsersListView));
        }
    }
}
