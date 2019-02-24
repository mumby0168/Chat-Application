using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialDesignThemes.Wpf;
using Networking.Client.Application.Events;
using Networking.Client.Application.Models;
using Networking.Client.Application.Network.Interfaces;
using Networking.Client.Application.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using User.System.Core.Model;

namespace Networking.Client.Application.ViewModels
{
    public class ChatRoomViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly INetworkConnectionController _networkConnectionController;
        private readonly ICurrentUser _currentUser;

        public ChatRoomViewModel(IEventAggregator eventAggregator, INetworkConnectionController networkConnectionController, ICurrentUser currentUser)
        {
            _eventAggregator = eventAggregator;
            _networkConnectionController = networkConnectionController;
            _currentUser = currentUser;
            _eventAggregator.GetEvent<UserLoginEvent>().Subscribe(UserLogin);        

            ServerModel = new ServerModel();
            ServerModel.IpAddress = "192.168.1.97";
            ServerModel.Port = 2500;
            ServerModel.ServerStatus = ServerStatus.Disconnected;

            ServerConnectCommand = new DelegateCommand(Connect);

            ToggleBaseCommand = new DelegateCommand<object>(ToggleBaseColor);
        }

        //COMMANDS
        public DelegateCommand ServerConnectCommand { get; set; }

        public DelegateCommand<object> ToggleBaseCommand { get; set; }

        #region Properties

        private SocketUser _user;

        public SocketUser User
        {
            get { return _user; }
            set => SetProperty(ref _user, value);
        }

        private ServerModel _serverModel;

        public ServerModel ServerModel      
        {
            get { return _serverModel; }
            set => SetProperty(ref _serverModel, value);
        }


        #endregion

        //COMMAND METHODS

        public void ToggleBaseColor(object value)
        {
            _eventAggregator.GetEvent<ChangeBaseColourEvent>().Publish(!(bool)value);
        }

        public void Connect()
        {

            IPAddress ipAddress;
            if (!IPAddress.TryParse(ServerModel.IpAddress, out ipAddress))
            {
                MessageBox.Show("IP address invalid.");
                return;            
            }

            _networkConnectionController.Connect(new IPEndPoint(ipAddress, ServerModel.Port), _currentUser.Id,
                () =>
                {
                    _networkConnectionController.BeginListeningForMessages();
                    ServerModel.ServerStatus = ServerStatus.Connected;
                },
                () =>
                {
                    MessageBox.Show("Failed Connecting to the server.");
                    ServerModel.ServerStatus = ServerStatus.Failed;
                });
        }

        #region Event Methods

        public void UserLogin(SocketUser user)
        {
            User = user;
        }

        #endregion
    }
}
