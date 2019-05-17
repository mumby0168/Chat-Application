using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Networking.Client.Application.Config;
using Networking.Client.Application.Events;
using Networking.Client.Application.Services;
using Networking.Client.Application.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Sockets.DataStructures.Base;
using User.System.Core;
using User.System.Core.Model;
using User.System.Core.Repository;

namespace Networking.Client.Application.ViewModels
{
    public class EditProfileViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IFileProcessorService _fileProcessorService;
        private readonly IRegionManager _regionManager;

        public EditProfileViewModel(IEventAggregator eventAggregator, IFileProcessorService fileProcessorService, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _fileProcessorService = fileProcessorService;
            _regionManager = regionManager;
            _eventAggregator.GetEvent<EditUserEvent>().Subscribe(EditUserCalled);
            SelectImageCommand = new DelegateCommand(SelectImage);
            SaveCommand = new DelegateCommand(Save);
        }

        public DelegateCommand SelectImageCommand { get; set; }

        public DelegateCommand SaveCommand { get; set; }


        private SocketUser _socketUser;

        public SocketUser SocketUser        
        {
            get { return _socketUser; }
            set
            {
                _socketUser = value;
                RaisePropertyChanged();
            }
        }

        public void EditUserCalled(SocketUser socketUser)
        {
            SocketUser = socketUser;
        }

        public async void SelectImage()
        {
            var imagePath = _fileProcessorService.SelectFile();

            if(imagePath == null) return;

            SocketUser.ProfilePicture = await _fileProcessorService.GetBytesFromImage(imagePath);
        }

        public async void Save()
        {
            if (string.IsNullOrWhiteSpace(SocketUser.Name))
            {
                MessageBox.Show("Please enter a name");
                return;
            }

            using (var uow = new UnitOfWork(new SocketDbContext()))
            {
                var user = await uow.SocketUserRepo.GetAsync(SocketUser.Id);
                user.Name = SocketUser.Name;
                user.ProfilePicture = SocketUser.ProfilePicture;
                await uow.CompleteAsync();
            }

            _eventAggregator.GetEvent<UserEditedEvent>().Publish(SocketUser);
            _regionManager.RequestNavigate(RegionNames.LeftPanel, nameof(UsersListView));
        }

    }
}
