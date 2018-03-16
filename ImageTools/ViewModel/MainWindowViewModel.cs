using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ImageTools.Infrastructure.Messages;
using ImageTools.Model;

namespace ImageTools.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private bool _isSelectSourceFolderVisible;
        private bool _isSelectTargetFolderVisible;
        private bool _isCompressImagesVisible;

        public MainWindowViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            //messenger.Register<SourceFolderSelectedMessage>(this, SourceFolderSelectedMessageHandler);
            //messenger.Register<TargetFolderSelectedMessage>(this, TargetFolderSelectedMessageHandler);
            messenger.Register<FolderSelectedMessage>(this, FolderSelectedMessageHandler);

            IsSelectSourceFolderVisible = true;
            //IsSelectTargetFolderVisible = false;
            IsCompressImagesVisible = false;

            messenger.Send(new SetFolderTypeModeMessage(FolderType.Source));
        }
        
        public bool IsSelectSourceFolderVisible
        {
            get { return _isSelectSourceFolderVisible; }
            set { Set(ref _isSelectSourceFolderVisible, value); }
        }
        
        //public bool IsSelectTargetFolderVisible
        //{
        //    get { return _isSelectTargetFolderVisible; }
        //    set { Set(ref _isSelectTargetFolderVisible, value); }
        //}
        
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

            IsCompressImagesVisible = message.FolderType == FolderType.Target;
            IsSelectSourceFolderVisible = !IsCompressImagesVisible;
        }

        //private void SourceFolderSelectedMessageHandler(SourceFolderSelectedMessage message)
        //{
        //    IsSelectSourceFolderVisible = false;
        //    IsSelectTargetFolderVisible = true;
        //    IsCompressImagesVisible = false;
        //}

        //private void TargetFolderSelectedMessageHandler(TargetFolderSelectedMessage message)
        //{
        //    IsSelectSourceFolderVisible = false;
        //    IsSelectTargetFolderVisible = false;
        //    IsCompressImagesVisible = true;
        //}
    }
}