using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ImageTools.Model;
using ImageTools.Utilities;

namespace ImageTools.ViewModel
{
    public class SelectFolderViewModel : ViewModelBase
    {
        private readonly IFolderManager _folderManager;
        private string _selectedFolder;
        //private string _drillDownFolder;

        public SelectFolderViewModel(IFolderManager folderManager)
        {
            if (folderManager == null) throw new ArgumentNullException(nameof(folderManager));

            _folderManager = folderManager;

            DrillDownFolderCommand = new RelayCommand<string>(DrillDownFolderExecuted);

            Folders = new ObservableCollection<Folder>();
            SelectedFolder = string.Empty;
        }
        
        public ICommand DrillDownFolderCommand { get; }
        
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
        
        //public string DrillDownFolder
        //{
        //    get { return _drillDownFolder; }
        //    set { Set(ref _drillDownFolder, value); }
        //}

        public ObservableCollection<Folder> Folders { get; }

        private void LoadFolders(string currentFolder)
        {
            Folders.Clear();
            foreach (var subFolder in _folderManager.GetSubFolders(currentFolder))
            {
                Folders.Add(subFolder);
            }
        }

        private void DrillDownFolderExecuted(string folder)
        {
            if (Directory.Exists(folder))
            {
                LoadFolders(folder);
            }
        }
    }
}
