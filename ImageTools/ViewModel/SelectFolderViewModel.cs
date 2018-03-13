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
        private Folder _selectedFolder;

        public SelectFolderViewModel(IFolderManager folderManager)
        {
            if (folderManager == null) throw new ArgumentNullException(nameof(folderManager));

            _folderManager = folderManager;

            DrillDownFolderCommand = new RelayCommand<Folder>(DrillDownFolderExecuted);
            NavigateBackCommand = new RelayCommand(NavigateBackExecuted, NavigateBackCanExecute);

            Folders = new ObservableCollection<Folder>();
            SelectedFolder = Folder.DefaultFolder;
        }

        public ICommand DrillDownFolderCommand { get; }

        public ICommand NavigateBackCommand { get; }
        
        public Folder SelectedFolder
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

        private void LoadFolders(Folder parentFolder)
        {
            Folders.Clear();
            foreach (var subFolder in _folderManager.GetSubFolders(parentFolder.Path))
            {
                Folders.Add(subFolder);
            }
        }

        private void DrillDownFolderExecuted(Folder folder)
        {
            if (Directory.Exists(folder.Path))
            {
                LoadFolders(folder);
                SelectedFolder = folder;
            }
        }

        private bool NavigateBackCanExecute()
        {
            return SelectedFolder != null && !string.IsNullOrEmpty(SelectedFolder.ParentFolder.Path);
        }

        private void NavigateBackExecuted()
        {
            LoadFolders(SelectedFolder.ParentFolder);
            SelectedFolder = SelectedFolder.ParentFolder;
        }
    }
}
