using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.Config;
using Networking.Client.Application.EventArgs;
using Networking.Client.Application.Events;
using Networking.Client.Application.Views;
using Prism.Events;
using Prism.Regions;

namespace Networking.Client.Application.Services.Concrete
{
    public class OverlayService : IOverlayService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        public OverlayService(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
        }
        public void DisplayError(string title, List<string> messages = null)
        {
            _eventAggregator.GetEvent<UpdateErrorView>().Publish(new UpdateErrorViewEventArgs(title, messages));
            _regionManager.RequestNavigate(RegionNames.OverlayRegion, nameof(ErrorMessageView));
            _eventAggregator.GetEvent<ShowOverlay>().Publish();
        }

        public void RemoveOverlay()
        {
            _eventAggregator.GetEvent<RemoveOverlay>().Publish();            
        }
    }
}
