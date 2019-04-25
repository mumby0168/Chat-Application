using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Networking.Client.Application.Config;
using Networking.Client.Application.Properties;
using Networking.Client.Application.Services;
using Networking.Client.Application.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using User.System.Core;
using User.System.Core.Model;
using User.System.Core.Repository;
using User.System.Core.Services.Interfaces;

namespace Networking.Client.Application.ViewModels
{
    public class RegisterViewModel : BindableBase
    {
        private readonly IFileProcessorService _fileProcessorService;
        private readonly IRegionManager _regionManager;
        private readonly IPasswordProtectionService _passwordProtectionService;

        public RegisterViewModel(IFileProcessorService fileProcessorService, IRegionManager regionManager, IPasswordProtectionService passwordProtectionService)
        {
            _fileProcessorService = fileProcessorService;
            _regionManager = regionManager;
            _passwordProtectionService = passwordProtectionService;
            SelectImageCommand = new DelegateCommand(SelectImage);
            PasswordChangedCommand = new DelegateCommand<object>(PasswordChanged);
            RePasswordChangedCommand = new DelegateCommand<object>(RePasswordChanged);
            LoginCommand = new DelegateCommand(() => _regionManager.RequestNavigate(RegionNames.MainRegion, nameof(LoginView)));
            RegisterCommand = new DelegateCommand(Register);
            User = new SocketUser();
        }

        #region Commands

        public DelegateCommand RegisterCommand { get; set; }

        public DelegateCommand LoginCommand { get; set; }

        public DelegateCommand SelectImageCommand { get; set; }

        public DelegateCommand<object> PasswordChangedCommand { get; set; }

        public DelegateCommand<object> RePasswordChangedCommand { get; set; }

        #endregion

        #region Properties

        private SocketUser _user;

        public SocketUser User
        {
            get => _user;
            set { SetProperty(ref _user, value); }
        }

        private string _reEnterPassword;
        public string ReEnterPassword
        {
            get => _reEnterPassword;
            set { SetProperty(ref _reEnterPassword, value); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { SetProperty(ref _password, value); }
        }

        #endregion

        #region Command Methods

        public async void SelectImage()
        {
            var imagePath = _fileProcessorService.SelectFile();
            User.ProfilePicture = await _fileProcessorService.GetBytesFromImage(imagePath);
        }

        public void PasswordChanged(object pb)
        {
            Password = CastAndGetValueFromPasswordBox(pb);
        }

        public void RePasswordChanged(object pb)
        {
            ReEnterPassword = CastAndGetValueFromPasswordBox(pb);
        }

        public async void Register()
        {
            //TODO: Pre checks
            var res = await CheckData();
            if (!res) return;

            var (password, salt) = _passwordProtectionService.Encrypt(Password);
            User.Password = password;
            User.Salt = salt;
            
            using (var uow = new UnitOfWork(new SocketDbContext()))
            {
                await uow.SocketUserRepo.AddAsync(User);
                await uow.CompleteAsync();
            }

            MessageBox.Show("Account Creation Successful.");
            LoginCommand.Execute();
        }

        private async Task<bool> CheckData()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ReEnterPassword) ||
                string.IsNullOrWhiteSpace(User.Email) || string.IsNullOrWhiteSpace(User.Name))
                errors.Add("Please make sure none of the fields are empty.");

            if (Password != ReEnterPassword)
                    errors.Add("Passwords do not match");

            if (!new EmailAddressAttribute().IsValid(User.Email))
                errors.Add("Please enter a valid email address.");

            using (var uow = new UnitOfWork(new SocketDbContext()))
            {
                if (User.Email != null)
                    if (await uow.SocketUserRepo.SingleOrDefaultAsync(u => u.Email == User.Email) != null)
                        errors.Add("The username is already in use.");
            }

            if (errors.Count != 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors));
            }

            return (errors.Count == 0);
        }


        #endregion

        #region Event Methods



        #endregion


        private string CastAndGetValueFromPasswordBox(object pb)
        {
            var passwordBox = (PasswordBox) pb;
            return passwordBox.Password;
        }
    }
}
