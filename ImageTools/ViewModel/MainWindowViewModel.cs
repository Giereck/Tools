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

        public MainWindowViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            //messenger.Register<SourceFolderSelectedMessage>(this, SourceFolderSelectedMessageHandler);
            //messenger.Register<TargetFolderSelectedMessage>(this, TargetFolderSelectedMessageHandler);
            messenger.Register<FolderSelectedMessage>(this, FolderSelectedMessageHandler);

            IsSelectSourceFolderExpanded = true;
            IsSelectTargetFolderExpanded = false;
            IsCompressImagesVisible = false;

            //messenger.Send(new SetFolderTypeModeMessage(FolderType.Source));
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

        private void FolderSelectedMessageHandler(FolderSelectedMessage message)
        {
            if (message.FolderType == FolderType.Source)
            {
                _messenger.Send(new SetFolderTypeModeMessage(FolderType.Target));
            }

            //IsCompressImagesVisible = message.FolderType == FolderType.Target;
            //IsSelectSourceFolderExpanded = !IsCompressImagesVisible;
        }

        //private void SourceFolderSelectedMessageHandler(SourceFolderSelectedMessage message)
        //{
        //    IsSelectSourceFolderExpanded = false;
        //    IsSelectTargetFolderExpanded = true;
        //    IsCompressImagesVisible = false;
        //}

        //private void TargetFolderSelectedMessageHandler(TargetFolderSelectedMessage message)
        //{
        //    IsSelectSourceFolderExpanded = false;
        //    IsSelectTargetFolderExpanded = false;
        //    IsCompressImagesVisible = true;
        //}
    }
}