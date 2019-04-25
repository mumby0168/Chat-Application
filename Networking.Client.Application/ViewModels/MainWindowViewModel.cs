using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.Config;
using Networking.Client.Application.Events;
using Networking.Client.Application.Views;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Networking.Client.Application.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {        
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {            
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(LoginView));
            _regionManager.RegisterViewWithRegion(RegionNames.LeftPanel, typeof(UsersListView));
            _regionManager.RegisterViewWithRegion(RegionNames.OverlayRegion, typeof(ErrorMessageView));
            _eventAggregator.GetEvent<RemoveOverlay>().Subscribe(() => IsOverlayVisible = false);
            _eventAggregator.GetEvent<ShowOverlay>().Subscribe(() => IsOverlayVisible = true);
        }
            
        private bool _isOverlayVisible;

        public bool IsOverlayVisible
        {
            get { return _isOverlayVisible; }
            set
            {
                _isOverlayVisible = value;
                OnPropertyChanged();
            }
        }

    }
}
