using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ImageTools.Infrastructure.Messages;
using ImageTools.Model;

namespace ImageTools.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private bool _isSelectSourceFolderExpanded;
        private bool _isSelectTargetFolderExpanded;
        private bool _isCompressImagesVisible;
        private string _selectedSourceFolderPath;
        private string _selectedTargetFolderPath;

        public MainWindowViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            messenger.Register<FolderSelectedMessage>(this, FolderSelectedMessageHandler);

            IsSelectSourceFolderExpanded = true;
            IsSelectTargetFolderExpanded = false;
            IsCompressImagesVisible = false;
        }
        
        public bool IsSelectSourceFolderExpanded
        {
            get { return _isSelectSourceFolderExpanded; }
            set { Set(ref _isSelectSourceFolderExpanded, value); }
        }

        public bool IsSelectTargetFolderExpanded
        {
            get { return _isSelectTargetFolderExpanded; }
            set { Set(ref _isSelectTargetFolderExpanded, value); }
        }

        public bool IsCompressImagesVisible
        {
            get { return _isCompressImagesVisible; }
            set { Set(ref _isCompressImagesVisible, value); }
        }
        
        public string SelectedSourceFolderPath
        {
            get { return _selectedSourceFolderPath; }
            set { Set(ref _selectedSourceFolderPath, value); }
        }
        
        public string SelectedTargetFolderPath
        {
            get { return _selectedTargetFolderPath; }
            set { Set(ref _selectedTargetFolderPath, value); }
        }

        private void FolderSelectedMessageHandler(FolderSelectedMessage message)
        {
            switch (message.FolderType)
            {
                case FolderType.Source:
                    SelectedSourceFolderPath = string.Format($"Source folder: {message.FolderPath}");
                    IsSelectSourceFolderExpanded = false;
                    if (string.IsNullOrEmpty(SelectedTargetFolderPath))
                    {
                        IsSelectTargetFolderExpanded = true;
                    }
                    break;
                case FolderType.Target:
                    SelectedTargetFolderPath = string.Format($"Target folder: {message.FolderPath}");
                    IsSelectTargetFolderExpanded = false;
                    if (string.IsNullOrEmpty(SelectedSourceFolderPath))
                    {
                        IsSelectSourceFolderExpanded = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Invalid folder type.");
            }

            if (!string.IsNullOrEmpty(SelectedSourceFolderPath) && !string.IsNullOrEmpty(SelectedTargetFolderPath))
            {
                IsCompressImagesVisible = true;
            }
        }
    }
}