using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        private readonly IFileProcessorService _fileProcessorService;


        public ChatViewModel(IEventAggregator eventAggregator, IChatManager chatManager, ICurrentUser currentUser, INetworkConnectionController networkConnectionController, IFileProcessorService fileProcessorService)
        {
            _eventAggregator = eventAggregator;
            _chatManager = chatManager;
            _currentUser = currentUser;
            _networkConnectionController = networkConnectionController;
            _fileProcessorService = fileProcessorService;
            _eventAggregator.GetEvent<UserSelected>().Subscribe(UserSelected);
            _chatManager.NewMessageCallback(UpdateChat);

            Images = new ObservableCollection<byte[]>();
            
            ChatMessages = new ObservableCollection<object>();           
            
            SendMessageCommand = new DelegateCommand(async () => await SendMessage());

            KeyDownCommand = new DelegateCommand(OnKeyDown);

            SelectImageCommand = new DelegateCommand(SelectImage);

            _networkConnectionController.MessageReceivedEventHandler += MessageReceivedEventHandler;

            _eventAggregator.GetEvent<LogoffEvent>().Subscribe(Logoff);
        }

        private async void SelectImage()
        {
            string imagePath =_fileProcessorService.SelectFile();
            byte[] image = await _fileProcessorService.GetBytesFromImage(imagePath);

            if (image.Length > ushort.MaxValue)
            {
                MessageBox.Show("Your image is to large!");
                return; 
            }

            ImageCount++;
            Images.Add(image);
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

        private int _imageCount;

        public int ImageCount
        {
            get { return _imageCount; }
            set
            {
                _imageCount = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<object> _chatMessages;
        public ObservableCollection<object> ChatMessages
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
            
        private ObservableCollection<byte[]> _images;

        public ObservableCollection<byte[]> Images
        {
            get { return _images; }
            set { _images = value; OnPropertyChanged(); }
        }



        #endregion

        //COMMANDS
        public DelegateCommand SendMessageCommand { get; set; }

        public DelegateCommand KeyDownCommand { get; set; }
        
        public DelegateCommand SelectImageCommand { get; set; }


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

            if (Images.Any())
                await SendImages();

            await _chatManager.SendChatMessage(chatMessage);

            ImageCount = 0;
        }

        private async Task SendImages()
        {
            foreach (var image in Images)
            {
                await _chatManager.SendImageMessage(new ImageMessage
                {
                    ImageData = image,
                    UserFromId = (ushort) _currentUser.Id,
                    UserToId = (ushort) SocketUserId
                });
                }
        }


        //PRIVATE METHODS
        private void UserSelected(int id)
        {            
            if (_chatManager.Chats.TryGetValue(id, out var chat))
            {
                SocketUserId = id;

                ChatMessages = new ObservableCollection<object>(chat);
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
            ChatMessages = new ObservableCollection<object>();
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
