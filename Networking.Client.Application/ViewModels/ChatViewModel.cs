﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Networking.Client.Application.EventArgs;
using Networking.Client.Application.Events;
using Networking.Client.Application.Models;
using Networking.Client.Application.Network.Interfaces;
using Networking.Client.Application.Services;
using Networking.Client.Application.Static;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;
using User.System.Core.Model;

namespace Networking.Client.Application.ViewModels
{
    public class ChatViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IChatManager _chatManager;
        private readonly ICurrentUser _currentUser;
        private readonly INetworkConnectionController _networkConnectionController;


        public ChatViewModel(IEventAggregator eventAggregator, IChatManager chatManager, ICurrentUser currentUser, INetworkConnectionController networkConnectionController)
        {
            _eventAggregator = eventAggregator;
            _chatManager = chatManager;
            _currentUser = currentUser;
            _networkConnectionController = networkConnectionController;
            _eventAggregator.GetEvent<UserSelected>().Subscribe(UserSelected);
            _chatManager.NewMessageCallback(UpdateChat);
            
            ChatMessages = new ObservableCollection<ChatMessageModel>();           
            
            SendMessageCommand = new DelegateCommand(async () => await SendMessage());

            KeyDownCommand = new DelegateCommand(OnKeyDown);

            _networkConnectionController.MessageReceivedEventHandler += MessageReceivedEventHandler;

            _eventAggregator.GetEvent<LogoffEvent>().Subscribe(Logoff);
        }

        private void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Message.MessageType)
            {
                case MessageType.Typing:
                    UserTyping((e.Message as UserTypingMessage).UserTypingId);
                    break;
            }
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

        private bool _isTyping;

        public bool IsTyping
        {
            get { return _isTyping; }
            set
            {
                _isTyping = value; 
                RaisePropertyChanged();
            }
        }


        #endregion

        //COMMANDS
        public DelegateCommand SendMessageCommand { get; set; }

        public DelegateCommand KeyDownCommand { get; set; }


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

        private async void OnKeyDown()
        {
            if(SocketUserId != 0)
                await _networkConnectionController.SendMessage(new UserTypingMessage {UserTypingToId = (ushort) SocketUserId, UserTypingId = (ushort) _currentUser.Id});
        }

        private void UserTyping(int id)
        {
            if (id == SocketUserId)
            {
                MainThread.Dispatch(async () =>
                {
                    IsTyping = true;
                    await Task.Run(() => Thread.Sleep(300));
                    MainThread.Dispatch(() => IsTyping = false);
                });
            }            
        }
    }
}
