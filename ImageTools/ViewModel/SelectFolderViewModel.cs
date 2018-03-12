using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using ImageTools.Model;
using ImageTools.Utilities;

namespace ImageTools.ViewModel
{
    public class SelectFolderViewModel : ViewModelBase
    {
        private readonly IFolderManager _folderManager;
        private string _selectedFolder;

        public SelectFolderViewModel(IFolderManager folderManager)
        {
            if (folderManager == null) throw new ArgumentNullException(nameof(folderManager));

            _folderManager = folderManager;

            Folders = new ObservableCollection<Folder>();
            SelectedFolder = string.Empty;
        }
        
        public string SelectedFolder
        {
            get { return _selectedFolder; }
            set
            {
                if (Set(ref _selectedFolder, value))
                {
                    LoadFolders(_selectedFolder);
                }
            }
        }

        public ObservableCollection<Folder> Folders { get; }

        private void LoadFolders(string currentFolder)
        {
            Folders.Clear();
            foreach (var subFolder in _folderManager.GetSubFolders(currentFolder))
            {
                Folders.Add(subFolder);
            }
        }
    }
}
