using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.Events;
using Networking.Client.Application.Models;
using Networking.Client.Application.Network.Interfaces;
using Networking.Client.Application.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Sockets.DataStructures.Messages;
using User.System.Core.Model;

namespace Networking.Client.Application.ViewModels
{
    public class ChatViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IChatManager _chatManager;
        private readonly ICurrentUser _currentUser;


        public ChatViewModel(IEventAggregator eventAggregator, IChatManager chatManager, ICurrentUser currentUser)
        {
            _eventAggregator = eventAggregator;
            _chatManager = chatManager;
            _currentUser = currentUser;
            _eventAggregator.GetEvent<UserSelected>().Subscribe(UserSelected);
            _chatManager.NewMessageCallback(UpdateChat);
            SocketUser = new SocketUser();
            ChatMessages = new ObservableCollection<ChatMessageModel>();
            ChatMessages.Add(new ChatMessageModel()
            {
                IsSent = false,
                Message = "Hello there",
                TimeStamp = DateTime.Now
            });
            SendMessageCommand = new DelegateCommand(async () => await SendMessage());
        }


        #region Properties

        private ObservableCollection<ChatMessageModel> _chatMessages;
        public ObservableCollection<ChatMessageModel> ChatMessages
        {
            get => _chatMessages;
            set { _chatMessages = value; RaisePropertyChanged();}
        }

        public SocketUser SocketUser { get; set; }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value; 
                RaisePropertyChanged();
            }
        }

            
        #endregion


        //COMMANDS
        public DelegateCommand SendMessageCommand { get; set; }



        //COMMAND METHODS
        public async Task SendMessage()
        {
            if(string.IsNullOrWhiteSpace(Message)) Console.WriteLine("Please enter a message to send.");

            var chatMessage = new ChatMessage()
            {
                Message = Message,
                UserFromId = (ushort) _currentUser.Id,
                UserToId = (ushort) SocketUser.Id
            };

            await _chatManager.SendChatMessage(chatMessage);
        }


        //PRIVATE METHODS
        private void UserSelected(int id)
        {
            //TODO: Fix Returns            

            var chat = _chatManager.Chats.FirstOrDefault(c => c.Key.Id == id);

            if (chat.Value == null) return;

            if (chat.Key == null) return;

            SocketUser = chat.Key;

            ChatMessages = new ObservableCollection<ChatMessageModel>(chat.Value);
        }

        private void UpdateChat(SocketUser socketUser)
        {
            if (socketUser.Id == SocketUser.Id)
            {
                UserSelected(socketUser.Id);
            }
        }
    }
}
