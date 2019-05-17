using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MaterialDesignThemes.Wpf;
using Networking.Client.Application.Config;
using Networking.Client.Application.Events;
using Networking.Client.Application.Models;
using Networking.Client.Application.Network.Interfaces;
using Networking.Client.Application.Services;
using Networking.Client.Application.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Sockets.DataStructures.Messages;
using User.System.Core.Model;

namespace Networking.Client.Application.ViewModels
{
    public class ChatRoomViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly INetworkConnectionController _networkConnectionController;
        private readonly ICurrentUser _currentUser;
        private readonly IChatManager _chatManager;
        private readonly IRegionManager _regionManager;
        private readonly IOverlayService _overlayService;

        /// <summary>
        /// Creates an instance of the chat room view model and resolves its dependencies
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <param name="networkConnectionController"></param>
        /// <param name="currentUser"></param>
        /// <param name="chatManager"></param>
        /// <param name="regionManager"></param>
        /// <param name="overlayService"></param>
        public ChatRoomViewModel(IEventAggregator eventAggregator, INetworkConnectionController networkConnectionController, ICurrentUser currentUser, IChatManager chatManager, IRegionManager regionManager, IOverlayService overlayService)
        {
            //assigns local variables
            _eventAggregator = eventAggregator;
            _networkConnectionController = networkConnectionController;
            _currentUser = currentUser;
            _chatManager = chatManager;
            _regionManager = regionManager;
            _overlayService = overlayService;

            _eventAggregator.GetEvent<UserLoginEvent>().Subscribe(UserLogin);
            _eventAggregator.GetEvent<OfflineUsersLoadedEvent>().Subscribe(() => IsConnectAllowed = true);
            _eventAggregator.GetEvent<UserEditedEvent>().Subscribe((user) => User = user);

            //Sets up the basic server settings
            ServerModel = new ServerModel();
            ServerModel.IpAddress = "192.168.1.97";
            ServerModel.Port = 2500;
            ServerModel.ServerStatus = ServerStatus.Disconnected;

            //Sets up command executions
            ServerConnectCommand = new DelegateCommand(Connect);
            ToggleBaseCommand = new DelegateCommand<object>(ToggleBaseColor);
            CurrentUserClickedCommand = new DelegateCommand(CurrentUserClicked);
            LogoutCommand = new DelegateCommand(Logout);
        }

        //COMMANDS
        public DelegateCommand ServerConnectCommand { get; set; }

        public DelegateCommand<object> ToggleBaseCommand { get; set; }

        public DelegateCommand LogoutCommand { get; set; }

        public DelegateCommand CurrentUserClickedCommand { get; set; }

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


        private bool _isConnectAllowed;

        public bool IsConnectAllowed
        {
            get { return _isConnectAllowed; }
            set
            {
                _isConnectAllowed = value;
                RaisePropertyChanged();
            }
        }



        #endregion

        //COMMAND METHODS
        
        /// <summary>
        /// Logs the user out of their account and disconnects them from the server.
        /// </summary>
        public void Logout()
        {
            _networkConnectionController.SendMessage(new UserLogoffMessage {UsersId = (ushort) _currentUser.Id});
            _chatManager.Chats = new Dictionary<int, List<object>>();
            _networkConnectionController.Disconnect();
            ServerModel.ServerStatus = ServerStatus.Disconnected;
            _regionManager.RequestNavigate(RegionNames.MainRegion, nameof(LoginView));         
            _eventAggregator.GetEvent<LogoffEvent>().Publish();
            IsConnectAllowed = false;
        }

        /// <summary>
        /// Toggles the base color.
        /// </summary>
        /// <param name="value"></param>
        public void ToggleBaseColor(object value)
        {
            _eventAggregator.GetEvent<ChangeBaseColourEvent>().Publish(!(bool)value);
        }

        /// <summary>
        /// Connects the user to the server.
        /// </summary>
        public void Connect()
        {

            IPAddress ipAddress;
            if (!IPAddress.TryParse(ServerModel.IpAddress, out ipAddress))
            {
                _overlayService.DisplayError("Invalid IP Address", new List<string>{"Please check the IP Address entered."});
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
                    _overlayService.DisplayError("Connection Failed", new List<string>{"Please check the IP Address.", "Please check the port.", "The server may be down."});
                    ServerModel.ServerStatus = ServerStatus.Failed;
                });
        }

        /// <summary>
        /// Executes when the current user changes.
        /// </summary>
        public void CurrentUserClicked()
        {
            _regionManager.RequestNavigate(RegionNames.LeftPanel, nameof(EditProfileView));
            _eventAggregator.GetEvent<EditUserEvent>().Publish(User);
        }

        #region Event Methods
        /// <summary>
        /// Executes when the user logs in.
        /// </summary>
        /// <param name="user"></param>
        public void UserLogin(SocketUser user)
        {
            User = user;
        }

        #endregion
    }
}
