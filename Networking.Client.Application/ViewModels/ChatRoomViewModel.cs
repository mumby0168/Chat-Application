using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.Events;
using Prism.Events;
using Prism.Mvvm;
using User.System.Core.Model;

namespace Networking.Client.Application.ViewModels
{
    public class ChatRoomViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public ChatRoomViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UserLoginEvent>().Subscribe(UserLogin);
        }

        #region Properties

        private SocketUser _user;

        public SocketUser User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }


        #endregion

        #region Event Methods

        public void UserLogin(SocketUser user)
        {
            User = user;
        }

        #endregion
    }
}
