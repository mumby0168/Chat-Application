using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Networking.Client.Application.EventArgs;
using Networking.Client.Application.Network.Interfaces;
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
        
        private List<SocketUser> _users = new List<SocketUser>();
        private ObservableCollection<SocketUser> _socketUsers;

        public UsersListViewModel(INetworkConnectionController networkConnectionController)
        {            
            _networkConnectionController = networkConnectionController;
            _networkConnectionController.MessageReceivedEventHandler += NewMessageFromServer;                   
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

        public async void NewMessageFromServer(object sender, MessageReceivedEventArgs args)
        {            
            switch (args.Message.MessageType)
            {
                case MessageType.NewUserOnline:
                    await NewUserOnline(args.Message as NewUserOnlineMessage);
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
                        MessageBox.Show(user.Email);
                   });
                }
                    
            }
        }

    }
}
