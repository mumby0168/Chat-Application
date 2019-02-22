﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Networking.Client.Application.Config;
using Networking.Client.Application.Events;
using Networking.Client.Application.Network.Interfaces;
using Networking.Client.Application.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using User.System.Core;
using User.System.Core.Model;
using User.System.Core.Repository;
using User.System.Core.Services.Interfaces;
using Image = System.Drawing.Image;

namespace Networking.Client.Application.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IPasswordProtectionService _passwordProtectionService;
        private readonly IEventAggregator _eventAggregator;
        private readonly INetworkConnectionController _networkConnectionController;

        public LoginViewModel(IRegionManager regionManager, IPasswordProtectionService passwordProtectionService, IEventAggregator eventAggregator, INetworkConnectionController networkConnectionController)
        {
            _regionManager = regionManager;
            _passwordProtectionService = passwordProtectionService;
            _eventAggregator = eventAggregator;
            _networkConnectionController = networkConnectionController;
            PasswordChangedCommand =new DelegateCommand<object>(PasswordChanged);
            LoginCommand = new DelegateCommand(Login);
            RegisterCommand = new DelegateCommand(Register);                 
        }

        #region Commands

        public DelegateCommand<object> PasswordChangedCommand { get; set; }

        public DelegateCommand LoginCommand { get; set; }

        public DelegateCommand RegisterCommand { get; set; }

        #endregion


        #region Properties

        private bool _isLoading;
            
        public bool IsLoading
        {
            get { return _isLoading; }
            set {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }


        private string _username;

        public string Username
        {
            get => _username;
            set
            {             
                if(_username != value)
                    SetProperty(ref _username, value);
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {   if(_password != value)                 
                    SetProperty(ref _password, value);
            }
        }



        #endregion

        #region Commands Methods

        public void PasswordChanged(object passwordBox)
        {
           var passwordBoxType = (PasswordBox) passwordBox;
            Password = passwordBoxType.Password;
        }

        public async void Login()
        {
            IsLoading = true;
            await LoginAsync();
            IsLoading = false;
        }

        private async Task LoginAsync()
        { 
            await Task.Run(() => Thread.Sleep(1000));
            SocketUser user;
            using (var uow = new UnitOfWork(new SocketDbContext()))
            {
                user = await uow.SocketUserRepo.SingleOrDefaultAsync(u => u.Email == Username);
            }

            if (user == null)
            {
                MessageBox.Show($"Username: {Username} not found");
                return;
            }

            if (_passwordProtectionService.Check(Password, user.Password, user.Salt))
            {
                //navigate to new view.                

                _networkConnectionController.Connect(new IPEndPoint(IPAddress.Parse("172.20.10.10"), 2500), user.Id,
                    () => { _networkConnectionController.BeginListeningForMessages(); },
                    () => { MessageBox.Show("Failed Connecting to the server."); });                

                _regionManager.RequestNavigate(RegionNames.MainRegion, nameof(ChatRoomView));
                _eventAggregator.GetEvent<UserLoginEvent>().Publish(user);
            }
            else
            {
                //say password incorrect.
                MessageBox.Show("Password Incorrect");
            }
        }

        public void Register()
        {
            _regionManager.RequestNavigate(RegionNames.MainRegion, nameof(RegisterView));
        }


        #endregion
    }
}
