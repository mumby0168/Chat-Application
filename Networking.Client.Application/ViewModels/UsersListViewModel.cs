using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Networking.Client.Application.EventArgs;
using Networking.Client.Application.Events;
using Networking.Client.Application.Models;
using Networking.Client.Application.Network.Interfaces;
using Networking.Client.Application.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;
using User.System.Core;
using User.System.Core.Model;
using User.System.Core.Repository;

namespace Networking.Client.Application.ViewModels
{
    public class UsersListViewModel : BindableBase
    {
        private readonly INetworkConnectionController _networkConnectionController;
        private readonly IEventAggregator _eventAggregator;
        private readonly IChatManager _chatManager;
        private readonly ICurrentUser _currentUser;

        private ObservableCollection<SocketUser> _socketUsers;

        public UsersListViewModel(INetworkConnectionController networkConnectionController, IEventAggregator eventAggregator, IChatManager chatManager, ICurrentUser currentUser)
        {            
            _networkConnectionController = networkConnectionController;
            _eventAggregator = eventAggregator;
            _chatManager = chatManager;
            _currentUser = currentUser;

            _networkConnectionController.MessageReceivedEventHandler += NewMessageFromServer;
            _eventAggregator.GetEvent<LogoffEvent>().Subscribe(Logoff);

            SelectedSocketUser = new SocketUser();
            OnlineSocketUsers = new ObservableCollection<SocketUser>();   
            OfflineSocketUsers = new ObservableCollection<SocketUser>();

            ViewLoadedCommand = new DelegateCommand(LoadView);
        }

        public DelegateCommand ViewLoadedCommand { get; set; }        

        public ObservableCollection<SocketUser> OnlineSocketUsers
        {
            get => _socketUsers;
            set
            {
                _socketUsers = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<SocketUser> _offlineSocketUsers;

        public ObservableCollection<SocketUser> OfflineSocketUsers
        {
            get { return _offlineSocketUsers; }
            set
            {
                _offlineSocketUsers = value;
                RaisePropertyChanged();
            }
        }
            

        private SocketUser _selectedSocketUser;

        public SocketUser SelectedSocketUser
        {
            get { return _selectedSocketUser; }
            set
            {
                if (value != null)
                {
                    value.IsMessageUnRead = false;
                    _selectedSocketUser = value;
                    UserSelected();
                }                
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }



        public async void NewMessageFromServer(object sender, MessageReceivedEventArgs args)
        {            
            switch (args.Message.MessageType)
            {
                case MessageType.NewUserOnline:
                    await NewUserOnline(args.Message as NewUserOnlineMessage);
                    break;
                case MessageType.Chat:
                    NewChat(((ChatMessage)args.Message).UserFromId);
                    break;
                case MessageType.UserOffline:
                    RemoveUserFromList((args.Message as UserOfflineMessage).UsersId);        
                    break;                    
            }   
        }

        private void RemoveUserFromList(int userId)
        {
            Debug.WriteLine("Remove user with id: " + userId);
            var user = OnlineSocketUsers.FirstOrDefault(s => s.Id == userId);            

            DispatchOnUiThread(() =>
            {
                OnlineSocketUsers.Remove(user);
                OfflineSocketUsers.Add(user);
            });

            if (OnlineSocketUsers.Count > 0)
                SelectedSocketUser = OnlineSocketUsers.First();
        }

        private async Task NewUserOnline(NewUserOnlineMessage newUserOnlineMessage)
        {                                                               
            DispatchOnUiThread(() => ChangeUserToOnline(newUserOnlineMessage.UserId));

            if (!_chatManager.Chats.ContainsKey(newUserOnlineMessage.UserId))
            {                        
                _chatManager.Chats.Add(newUserOnlineMessage.UserId, new List<ChatMessageModel>());
            }                                              
        }

        private void NewChat(int userFromId)
        {
            if(SelectedSocketUser.Id == userFromId) return;

            var user = OnlineSocketUsers.FirstOrDefault(s => s.Id == userFromId);
            if (user != null) user.IsMessageUnRead = true;            
        }

        private void ChangeUserToOnline(int id)
        {
            var user = OfflineSocketUsers.FirstOrDefault(s => s.Id == id);
            OfflineSocketUsers.Remove(user);

            if(user == null) return;
            
            OnlineSocketUsers.Add(user);
        }        


        private void UserSelected()
        {
            _eventAggregator.GetEvent<UserSelected>().Publish(SelectedSocketUser.Id);            
        }

        private void Logoff()
        {
            OnlineSocketUsers.Clear();
            SelectedSocketUser = new SocketUser();
        }

        private void DispatchOnUiThread(Action function)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(function);
        }

        private async void LoadView()
        {
            IsLoading = true;
            using (var uow = new UnitOfWork(new SocketDbContext()))
            {
                var users = await uow.SocketUserRepo.GetAllAsync();
                var socketUsers = users.ToList();
                var currentUser = socketUsers.FirstOrDefault(c => c.Id == _currentUser.Id);
                socketUsers.Remove(currentUser);
                OfflineSocketUsers = new ObservableCollection<SocketUser>(socketUsers);                
            }

            IsLoading = false;

            _eventAggregator.GetEvent<OfflineUsersLoadedEvent>().Publish();
        }
    }
}
