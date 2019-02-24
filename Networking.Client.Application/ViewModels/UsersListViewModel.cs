using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Networking.Client.Application.EventArgs;
using Networking.Client.Application.Events;
using Networking.Client.Application.Models;
using Networking.Client.Application.Network.Interfaces;
using Prism.Events;
using Prism.Mvvm;
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

        private List<SocketUser> _users = new List<SocketUser>();
        private ObservableCollection<SocketUser> _socketUsers;

        public UsersListViewModel(INetworkConnectionController networkConnectionController, IEventAggregator eventAggregator, IChatManager chatManager)
        {            
            _networkConnectionController = networkConnectionController;
            _eventAggregator = eventAggregator;
            _chatManager = chatManager;
            _networkConnectionController.MessageReceivedEventHandler += NewMessageFromServer;
            SelectedSocketUser = new SocketUser();
                SocketUsers = new ObservableCollection<SocketUser>();
            SocketUsers.Add(new SocketUser(){Name = "Billy Mumby", Email = "billy.mumby@outlook.com"});
            
        }

        public ObservableCollection<SocketUser> SocketUsers
        {
            get => _socketUsers;
            set
            {
                _socketUsers = value;
                RaisePropertyChanged();
            }
        }

        private SocketUser _selectedSocketUser;

        public SocketUser SelectedSocketUser
        {
            get { return _selectedSocketUser; }
            set
            {
                value.IsMessageUnRead = false;
                _selectedSocketUser = value;
                UserSelected();
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
            }
        }

        private async Task NewUserOnline(NewUserOnlineMessage newUserOnlineMessage)
        {
            using (var uow = new UnitOfWork(new SocketDbContext()))
            {
                var user = await uow.SocketUserRepo.SingleOrDefaultAsync(u => u.Id == newUserOnlineMessage.UserId);
                if (user != null)
                {                    
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                   {                                           
                        _users.Add(user);
                        SocketUsers = new ObservableCollection<SocketUser>(_users);                        
                   });

                    if (!_chatManager.Chats.ContainsKey(user))
                    {
                        Debug.WriteLine("new chat user added to dictionary.");
                        _chatManager.Chats.Add(user, new List<ChatMessageModel>());
                    }
                }
                    
            }
        }

        private void NewChat(int userFromId)
        {
            if(SelectedSocketUser.Id == userFromId) return;

            var user = SocketUsers.FirstOrDefault(s => s.Id == userFromId);
            if (user != null) user.IsMessageUnRead = true;            
        }

        private void UserSelected()
        {
            _eventAggregator.GetEvent<UserSelected>().Publish(SelectedSocketUser.Id);            
        }

    }
}
