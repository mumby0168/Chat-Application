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
            
            ChatMessages = new ObservableCollection<ChatMessageModel>();
            //ChatMessages.Add(new ChatMessageModel()
            //{
            //    IsSent = false,
            //    Message = "Hello there",
            //    TimeStamp = DateTime.Now
            //});
            //ChatMessages.Add(new ChatMessageModel()
            //{
            //    IsSent = true,
            //    Message = "Hello mate",
            //    TimeStamp = DateTime.Now
            //});
            SendMessageCommand = new DelegateCommand(async () => await SendMessage());
            _eventAggregator.GetEvent<LogoffEvent>().Subscribe(Logoff);
        }


        #region Properties

        private ObservableCollection<ChatMessageModel> _chatMessages;
        public ObservableCollection<ChatMessageModel> ChatMessages
        {
            get => _chatMessages;
            set { _chatMessages = value; RaisePropertyChanged();}
        }

        public int SocketUserId { get; set; }

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
                UserToId = (ushort) SocketUserId
            };

            await _chatManager.SendChatMessage(chatMessage);
        }


        //PRIVATE METHODS
        private void UserSelected(int id)
        {
            //TODO: Fix Returns            

            if (_chatManager.Chats.TryGetValue(id, out var chat))
            {
                SocketUserId = id;

                ChatMessages = new ObservableCollection<ChatMessageModel>(chat);
            }            
        }

        private void UpdateChat(int id)
        {
            if (id == SocketUserId)
            {
                UserSelected(id);
            }
        }

        private void Logoff()
        {
            SocketUserId = 0;
            ChatMessages = new ObservableCollection<ChatMessageModel>();
            Message = "";
        }
    }
}
