using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.EventArgs;
using Networking.Client.Application.Events;
using Networking.Client.Application.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Networking.Client.Application.ViewModels
{
    public class ErrorMessageViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IOverlayService _overlayService;
        private string _title;
        private List<string> _messages;

        public ErrorMessageViewModel(IEventAggregator eventAggregator, IOverlayService overlayService)
        {
            _eventAggregator = eventAggregator;
            _overlayService = overlayService;
            _eventAggregator.GetEvent<UpdateErrorView>().Subscribe(OnUpdateErrorView);
            CloseCommand = new DelegateCommand(() => _overlayService.RemoveOverlay());
        }

        public DelegateCommand CloseCommand { get; set; }

        private void OnUpdateErrorView(UpdateErrorViewEventArgs obj)
        {
            Title = obj.Title;
            Messages = obj.Messages;
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public List<string> Messages
        {
            get => _messages;
            set
            {
                _messages = value; 
                OnPropertyChanged();
            }
        }
    }
}
